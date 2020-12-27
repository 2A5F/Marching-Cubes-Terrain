﻿using System;
using System.Collections.Generic;
using System.Linq;
using Eldemarkki.VoxelTerrain.Utilities;
using Eldemarkki.VoxelTerrain.Utilities.Intersection;
using Eldemarkki.VoxelTerrain.World;
using Eldemarkki.VoxelTerrain.World.Chunks;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Eldemarkki.VoxelTerrain.VoxelData
{
    /// <summary>
    /// A store which handles getting and setting the voxel data for the world
    /// </summary>
    public class VoxelDataStore : MonoBehaviour
    {
        /// <summary>
        /// A dictionary containing the chunks. Key is the chunk's coordinate, and the value is the chunk's voxel data array
        /// </summary>
        private Dictionary<int3, NativeArray<byte>> _chunks;

        /// <summary>
        /// A dictionary of all the ongoing voxel data generation jobs. Key is the chunk's coordinate, and the value is the ongoing job for that chunk
        /// </summary>
        private Dictionary<int3, JobHandleWithData<IVoxelDataGenerationJob>> _generationJobHandles;

        /// <summary>
        /// The world that "owns" this voxel data store
        /// </summary>
        public VoxelWorld VoxelWorld { get; set; }

        private void Awake()
        {
            _chunks = new Dictionary<int3, NativeArray<byte>>();
            _generationJobHandles = new Dictionary<int3, JobHandleWithData<IVoxelDataGenerationJob>>();
        }

        private void OnApplicationQuit()
        {
            if (_chunks != null)
            {
                foreach (NativeArray<byte> chunk in _chunks.Values)
                {
                    if (chunk.IsCreated)
                    {
                        chunk.Dispose();
                    }
                }
            }

            if (_generationJobHandles != null)
            {
                foreach (var jobHandle in _generationJobHandles.Values)
                {
                    jobHandle.JobHandle.Complete();
                    jobHandle.JobData.OutputVoxelData.Dispose();
                }

                _generationJobHandles.Clear();
            }
        }

        /// <summary>
        /// Moves a chunk from coordinate <paramref name="from"/> to the coordinate <paramref name="to"/> and starts generating the voxel data for the chunk at <paramref name="to"/>
        /// </summary>
        /// <param name="from">The coordinate to move the chunk from</param>
        /// <param name="to">The new coordinate of the chunk</param>
        public void MoveChunk(int3 from, int3 to)
        {
            // Check that 'from' and 'to' are not equal
            if (from.Equals(to)) { return; }

            // Check that a chunk exists at 'from'
            if (TryGetVoxelDataChunk(from, out NativeArray<byte> chunk))
            {
                StartGeneratingVoxelData(to, chunk);
            }

            if (DoesChunkExistAtCoordinate(to))
            {
                _chunks.Remove(from);
            }
        }

        /// <summary>
        /// Checks if a chunk exists or is currently being generated at the coordinate <paramref name="chunkCoordinate"/>
        /// </summary>
        /// <param name="chunkCoordinate">The coordinate to check for</param>
        /// <returns>Returns true if a chunk exists or is currently being generated at <paramref name="chunkCoordinate"/>, otherwise returns false</returns>
        private bool DoesChunkExistAtCoordinate(int3 chunkCoordinate)
        {
            return _chunks.ContainsKey(chunkCoordinate) || _generationJobHandles.ContainsKey(chunkCoordinate);
        }

        /// <summary>
        /// Starts generating the voxel data for the chunk at <paramref name="chunkCoordinate"/>, where the output array is <paramref name="outputVoxelDataArray"/>
        /// </summary>
        /// <param name="chunkCoordinate">The coordinate of the chunk to start generating data for</param>
        /// <param name="outputVoxelDataArray">The array where the data should be generated to; saves memory by not allocating a new one</param>
        private void StartGeneratingVoxelData(int3 chunkCoordinate, NativeArray<byte> outputVoxelDataArray)
        {
            if (!_generationJobHandles.ContainsKey(chunkCoordinate))
            {
                int3 chunkWorldOrigin = chunkCoordinate * VoxelWorld.WorldSettings.ChunkSize;
                JobHandleWithData<IVoxelDataGenerationJob> jobHandleWithData = VoxelWorld.VoxelDataGenerator.GenerateVoxelData(chunkWorldOrigin, VoxelWorld.WorldSettings.ChunkSize + 1, outputVoxelDataArray);
                SetVoxelDataJobHandle(jobHandleWithData, chunkCoordinate);
            }
        }

        /// <summary>
        /// Starts generating the voxel data for the chunk at <paramref name="chunkCoordinate"/> and generates a new voxel data array for the new data with a persistent allocator
        /// </summary>
        /// <param name="chunkCoordinate">The coordinate of the chunk to start generating data for</param>
        public void StartGeneratingVoxelData(int3 chunkCoordinate)
        {
            if (!_generationJobHandles.ContainsKey(chunkCoordinate))
            {
                BoundsInt chunkBounds = BoundsUtilities.GetChunkBounds(chunkCoordinate, VoxelWorld.WorldSettings.ChunkSize);
                JobHandleWithData<IVoxelDataGenerationJob> jobHandleWithData = VoxelWorld.VoxelDataGenerator.GenerateVoxelData(chunkBounds);
                SetVoxelDataJobHandle(jobHandleWithData, chunkCoordinate);
            }
        }

        /// <summary>
        /// Gets all the coordinates of the chunks that already exist or are currently being generated, where the Chebyshev distance from <paramref name="coordinate"/> to the chunk's coordinate is more than <paramref name="range"/>
        /// </summary>
        /// <param name="coordinate">The central coordinate where the distances should be measured from</param>
        /// <param name="range">The maximum allowed manhattan distance</param>
        /// <returns></returns>
        public IEnumerable<int3> GetChunkCoordinatesOutsideOfRange(int3 coordinate, int range)
        {
            int3[] chunksArray = _chunks.Keys.ToArray();
            for (int i = 0; i < chunksArray.Length; i++)
            {
                int3 chunkCoordinate = chunksArray[i];
                if (DistanceUtilities.ChebyshevDistanceGreaterThan(coordinate, chunkCoordinate, range))
                {
                    yield return chunkCoordinate;
                }
            }

            int3[] generationJobHandleArray = _generationJobHandles.Keys.ToArray();
            for (int i = 0; i < generationJobHandleArray.Length; i++)
            {
                int3 generationCoordinate = generationJobHandleArray[i];
                if (DistanceUtilities.ChebyshevDistanceGreaterThan(coordinate, generationCoordinate, range))
                {
                    yield return generationCoordinate;
                }
            }
        }

        /// <summary>
        /// Tries to get the voxel data from <paramref name="worldPosition"/>. If the position is not loaded, false will be returned and <paramref name="voxelData"/> will be set to 0 (Note that 0 doesn't directly mean that the position is not loaded). If it is loaded, true will be returned and <paramref name="voxelData"/> will be set to the value.
        /// </summary>
        /// <param name="worldPosition">The world position to get the voxel data from</param>
        /// <param name="voxelData">The voxel data value at the world position</param>
        /// <returns>Does a voxel data point exist at that position</returns>
        public bool TryGetVoxelData(int3 worldPosition, out byte voxelData)
        {
            int3 chunkCoordinate = VectorUtilities.WorldPositionToCoordinate(worldPosition, VoxelWorld.WorldSettings.ChunkSize);
            ApplyChunkChanges(chunkCoordinate);
            if (_chunks.TryGetValue(chunkCoordinate, out NativeArray<byte> chunk))
            {
                int3 voxelDataLocalPosition = worldPosition.Mod(VoxelWorld.WorldSettings.ChunkSize);
                return chunk.TryGetElement(VoxelWorld.WorldSettings.ChunkSize + 1, voxelDataLocalPosition, out voxelData);
            }
            else
            {
                voxelData = 0;
                return false;
            }
        }

        /// <summary>
        /// Tries to get the voxel data array for one chunk with a persistent allocator. If a chunk doesn't exist there, false will be returned and <paramref name="chunk"/> will be set to null. If a chunk exists there, true will be returned and <paramref name="chunk"/> will be set to the chunk.
        /// </summary>
        /// <param name="chunkCoordinate">The coordinate of the chunk whose voxel data should be gotten</param>
        /// <param name="chunk">The voxel data of a chunk at the coordinate</param>
        /// <returns>Does a chunk exists at that coordinate</returns>
        public bool TryGetVoxelDataChunk(int3 chunkCoordinate, out NativeArray<byte> chunk)
        {
            ApplyChunkChanges(chunkCoordinate);
            return _chunks.TryGetValue(chunkCoordinate, out chunk);
        }

        /// <summary>
        /// Gets the voxel data of a custom volume in the world
        /// </summary>
        /// <param name="worldSpaceQuery">The world-space volume to get the voxel data for</param>
        /// <param name="allocator">How the new voxel data array should be allocated</param>
        /// <returns>The voxel data array inside the bounds</returns>
        public NativeArray<byte> GetVoxelDataCustom(BoundsInt worldSpaceQuery, Allocator allocator)
        {
            NativeArray<byte> voxelDataArray = new NativeArray<byte>(worldSpaceQuery.CalculateVolume(), allocator);

            ForEachVoxelDataArrayInQuery(worldSpaceQuery, (chunkCoordinate, voxelDataChunk) =>
            {
                ForEachVoxelDataInQueryInChunk(worldSpaceQuery, chunkCoordinate, voxelDataChunk, (voxelDataWorldPosition, voxelDataLocalPosition, voxelDataIndex, voxelData) =>
                {
                    voxelDataArray.SetElement(voxelData, worldSpaceQuery.size.ToInt3(), voxelDataWorldPosition - worldSpaceQuery.min.ToInt3());
                });
            });

            return voxelDataArray;
        }

        /// <summary>
        /// Sets the voxel data for a world position
        /// </summary>
        /// <param name="voxelData">The new voxel data</param>
        /// <param name="worldPosition">The world position of the voxel data</param>
        public void SetVoxelData(byte voxelData, int3 worldPosition)
        {
            IEnumerable<int3> affectedChunkCoordinates = ChunkUtilities.GetChunkCoordinatesContainingPoint(worldPosition, VoxelWorld.WorldSettings.ChunkSize);

            foreach (int3 chunkCoordinate in affectedChunkCoordinates)
            {
                if (!_chunks.ContainsKey(chunkCoordinate)) { continue; }

                if (TryGetVoxelDataChunk(chunkCoordinate, out NativeArray<byte> voxelDataArray))
                {
                    int3 localPos = (worldPosition - chunkCoordinate * VoxelWorld.WorldSettings.ChunkSize).Mod(VoxelWorld.WorldSettings.ChunkSize + 1);
                    voxelDataArray.SetElement(voxelData, VoxelWorld.WorldSettings.ChunkSize + 1, localPos);

                    if (VoxelWorld.ChunkStore.TryGetChunkAtCoordinate(chunkCoordinate, out ChunkProperties chunkProperties))
                    {
                        chunkProperties.HasChanges = true;
                    }
                }
            }
        }

        /// <summary>
        /// Sets a chunk's voxel data
        /// </summary>
        /// <param name="newVoxelDataArray">The new voxel data</param>
        /// <param name="chunkCoordinate">The coordinate of the chunk whose voxel data should be set</param>
        public void SetVoxelDataChunk(NativeArray<byte> newVoxelDataArray, int3 chunkCoordinate)
        {
            if (_chunks.TryGetValue(chunkCoordinate, out NativeArray<byte> oldVoxelDataArray))
            {
                oldVoxelDataArray.CopyFrom(newVoxelDataArray);
                newVoxelDataArray.Dispose();
            }
            else
            {
                _chunks.Add(chunkCoordinate, newVoxelDataArray);
            }

            if (VoxelWorld.ChunkStore.TryGetChunkAtCoordinate(chunkCoordinate, out ChunkProperties chunkProperties))
            {
                chunkProperties.HasChanges = true;
            }
        }

        /// <summary>
        /// Sets the voxel data for a volume in the world
        /// </summary>
        /// <param name="voxelDataArray">The new voxel data array</param>
        /// <param name="originPosition">The world position of the origin where the voxel data should be set</param>
        public void SetVoxelDataCustom(NativeArray<byte> voxelDataArray, int3 voxelDataArrayDimensions, int3 originPosition)
        {
            BoundsInt worldSpaceQuery = new BoundsInt(originPosition.ToVectorInt(), (voxelDataArrayDimensions - new int3(1, 1, 1)).ToVectorInt());

            ForEachVoxelDataArrayInQuery(worldSpaceQuery, (chunkCoordinate, voxelDataChunk) =>
            {
                ForEachVoxelDataInQueryInChunk(worldSpaceQuery, chunkCoordinate, voxelDataChunk, (voxelDataWorldPosition, voxelDataLocalPosition, voxelDataIndex, voxelData) =>
                {
                    voxelDataChunk.SetElement(voxelData, voxelDataArrayDimensions, voxelDataWorldPosition - chunkCoordinate * VoxelWorld.WorldSettings.ChunkSize);
                });

                if (VoxelWorld.ChunkStore.TryGetChunkAtCoordinate(chunkCoordinate, out ChunkProperties chunkProperties))
                {
                    chunkProperties.HasChanges = true;
                }
            });
        }

        /// <summary>
        /// Sets the voxel data for a volume in the world
        /// </summary>
        /// <param name="worldSpaceQuery">The volume where the voxel datas should be set to</param>
        /// <param name="setVoxelDataFunction">The function that calculates what the voxel data should be set to at the specific location. The first argument is the world space position of the voxel data, and the second argument is the current voxel data. The return value is what the new voxel data should be set to.</param>

        public void SetVoxelDataCustom(BoundsInt worldSpaceQuery, Func<int3, byte, byte> setVoxelDataFunction)
        {
            ForEachVoxelDataArrayInQuery(worldSpaceQuery, (chunkCoordinate, voxelDataChunk) =>
            {
                bool anyChanged = false;
                ForEachVoxelDataInQueryInChunk(worldSpaceQuery, chunkCoordinate, voxelDataChunk, (voxelDataWorldPosition, voxelDataLocalPosition, voxelDataIndex, voxelData) =>
                {
                    byte newVoxelData = setVoxelDataFunction(voxelDataWorldPosition, voxelData);
                    if (newVoxelData != voxelData)
                    {
                        voxelDataChunk.SetElement(newVoxelData, voxelDataIndex);
                        anyChanged = true;
                    }
                });

                if (anyChanged)
                {
                    if (VoxelWorld.ChunkStore.TryGetChunkAtCoordinate(chunkCoordinate, out ChunkProperties chunkProperties))
                    {
                        chunkProperties.HasChanges = true;
                    }
                }
            });
        }

        /// <summary>
        /// Loops through each voxel data array that intersects with <paramref name="worldSpaceQuery"/> and performs <paramref name="function"/> on them.
        /// </summary>
        /// <param name="worldSpaceQuery">The query which will be used to determine all the chunks that should be looped through</param>
        /// <param name="function">The function that will be performed on every chunk. The arguments are as follows: 1) The chunk's coordinate, 2) The chunk's voxel data</param>
        public void ForEachVoxelDataArrayInQuery(BoundsInt worldSpaceQuery, Action<int3, NativeArray<byte>> function)
        {
            int3 chunkSize = VoxelWorld.WorldSettings.ChunkSize;

            int3 minChunkCoordinate = VectorUtilities.WorldPositionToCoordinate(worldSpaceQuery.min - Vector3Int.one, chunkSize);
            int3 maxChunkCoordinate = VectorUtilities.WorldPositionToCoordinate(worldSpaceQuery.max, chunkSize);

            for (int chunkCoordinateX = minChunkCoordinate.x; chunkCoordinateX <= maxChunkCoordinate.x; chunkCoordinateX++)
            {
                for (int chunkCoordinateY = minChunkCoordinate.y; chunkCoordinateY <= maxChunkCoordinate.y; chunkCoordinateY++)
                {
                    for (int chunkCoordinateZ = minChunkCoordinate.z; chunkCoordinateZ <= maxChunkCoordinate.z; chunkCoordinateZ++)
                    {
                        int3 chunkCoordinate = new int3(chunkCoordinateX, chunkCoordinateY, chunkCoordinateZ);
                        if (!TryGetVoxelDataChunk(chunkCoordinate, out NativeArray<byte> voxelDataChunk))
                        {
                            continue;
                        }

                        function(chunkCoordinate, voxelDataChunk);
                    }
                }
            }
        }

        /// <summary>
        /// Loops through each voxel data point that is contained in <paramref name="voxelDataChunk"/> AND in <paramref name="worldSpaceQuery"/>, and performs <paramref name="function"/> on it
        /// </summary>
        /// <param name="worldSpaceQuery">The query that determines whether or not a voxel data point is contained.</param>
        /// <param name="chunkCoordinate">The coordinate of <paramref name="voxelDataChunk"/></param>
        /// <param name="voxelDataChunk">The voxel datas of the chunk</param>
        /// <param name="function">The function that will be performed on each voxel data point. The arguments are as follows: 1) The world space position of the voxel data point, 2) The chunk space position of the voxel data point, 3) The index of the voxel data point inside of <paramref name="voxelDataChunk"/>, 4) The value of the voxel data</param>
        public void ForEachVoxelDataInQueryInChunk(BoundsInt worldSpaceQuery, int3 chunkCoordinate, NativeArray<byte> voxelDataChunk, Action<int3, int3, int, byte> function)
        {
            int3 chunkBoundsSize = VoxelWorld.WorldSettings.ChunkSize;
            int3 chunkWorldSpaceOrigin = chunkCoordinate * VoxelWorld.WorldSettings.ChunkSize;

            BoundsInt chunkWorldSpaceBounds = new BoundsInt(chunkWorldSpaceOrigin.ToVectorInt(), chunkBoundsSize.ToVectorInt());

            BoundsInt intersectionVolume = IntersectionUtilities.GetIntersectionVolume(worldSpaceQuery, chunkWorldSpaceBounds);
            int3 intersectionVolumeMin = intersectionVolume.min.ToInt3();
            int3 intersectionVolumeMax = intersectionVolume.max.ToInt3();

            for (int voxelDataWorldPositionX = intersectionVolumeMin.x; voxelDataWorldPositionX <= intersectionVolumeMax.x; voxelDataWorldPositionX++)
            {
                for (int voxelDataWorldPositionY = intersectionVolumeMin.y; voxelDataWorldPositionY <= intersectionVolumeMax.y; voxelDataWorldPositionY++)
                {
                    for (int voxelDataWorldPositionZ = intersectionVolumeMin.z; voxelDataWorldPositionZ <= intersectionVolumeMax.z; voxelDataWorldPositionZ++)
                    {
                        int3 voxelDataWorldPosition = new int3(voxelDataWorldPositionX, voxelDataWorldPositionY, voxelDataWorldPositionZ);

                        int3 voxelDataLocalPosition = voxelDataWorldPosition - chunkWorldSpaceOrigin;
                        int voxelDataIndex = IndexUtilities.XyzToIndex(voxelDataLocalPosition, chunkBoundsSize.x + 1, chunkBoundsSize.y + 1);
                        if (voxelDataChunk.TryGetElement(voxelDataIndex, out byte voxelData))
                        {
                            function(voxelDataWorldPosition, voxelDataLocalPosition, voxelDataIndex, voxelData);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the job handle for a chunk coordinate
        /// </summary>
        /// <param name="generationJobHandle">The job handle with data</param>
        /// <param name="chunkCoordinate">The coordinate of the chunk to set the job handle for</param>
        public void SetVoxelDataJobHandle(JobHandleWithData<IVoxelDataGenerationJob> generationJobHandle, int3 chunkCoordinate)
        {
            _generationJobHandles.Add(chunkCoordinate, generationJobHandle);
        }

        /// <summary>
        /// If the chunk coordinate has an ongoing voxel data generation job, it will get completed and it's result will be applied to the chunk
        /// </summary>
        /// <param name="chunkCoordinate">The coordinate of the chunk to apply changes for</param>
        private void ApplyChunkChanges(int3 chunkCoordinate)
        {
            if (_generationJobHandles.TryGetValue(chunkCoordinate, out JobHandleWithData<IVoxelDataGenerationJob> jobHandle))
            {
                jobHandle.JobHandle.Complete();
                SetVoxelDataChunk(jobHandle.JobData.OutputVoxelData, chunkCoordinate);
                _generationJobHandles.Remove(chunkCoordinate);
            }
        }
    }
}
