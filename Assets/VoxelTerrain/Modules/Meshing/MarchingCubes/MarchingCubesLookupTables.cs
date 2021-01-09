﻿namespace Eldemarkki.VoxelTerrain.Meshing.MarchingCubes
{
    /// <summary>
    /// A collection of lookup tables needed for the marching cubes algorithm
    /// </summary>
    public static class MarchingCubesLookupTables
    {
        /// <summary>
        /// Lookup table for how the edges should be connected
        /// </summary>
        public static readonly int[] EdgeIndexTable =
        {
            0, 1,
            1, 2,
            2, 3,
            3, 0,
            4, 5,
            5, 6,
            6, 7,
            7, 4,
            0, 4,
            1, 5,
            2, 6,
            3, 7
        };

        /// <summary>
        /// The triangle table for the marching cubes algorithm. Used to determine how the faces should be connected. The first value is how many triangles that row has (actual row length / 3), and the rest of the row is the actual data
        /// </summary>
        public static readonly int[] TriangleTableWithLengths =
        {
            0,
            1, 0, 8, 3,
            1, 0, 1, 9,
            2, 1, 8, 3, 9, 8, 1,
            1, 1, 2, 10,
            2, 0, 8, 3, 1, 2, 10,
            2, 9, 2, 10, 0, 2, 9,
            3, 2, 8, 3, 2, 10, 8, 10, 9, 8,
            1, 3, 11, 2,
            2, 0, 11, 2, 8, 11, 0,
            2, 1, 9, 0, 2, 3, 11,
            3, 1, 11, 2, 1, 9, 11, 9, 8, 11,
            2, 3, 10, 1, 11, 10, 3,
            3, 0, 10, 1, 0, 8, 10, 8, 11, 10,
            3, 3, 9, 0, 3, 11, 9, 11, 10, 9,
            2, 9, 8, 10, 10, 8, 11,
            1, 4, 7, 8,
            2, 4, 3, 0, 7, 3, 4,
            2, 0, 1, 9, 8, 4, 7,
            3, 4, 1, 9, 4, 7, 1, 7, 3, 1,
            2, 1, 2, 10, 8, 4, 7,
            3, 3, 4, 7, 3, 0, 4, 1, 2, 10,
            3, 9, 2, 10, 9, 0, 2, 8, 4, 7,
            4, 2, 10, 9, 2, 9, 7, 2, 7, 3, 7, 9, 4,
            2, 8, 4, 7, 3, 11, 2,
            3, 11, 4, 7, 11, 2, 4, 2, 0, 4,
            3, 9, 0, 1, 8, 4, 7, 2, 3, 11,
            4, 4, 7, 11, 9, 4, 11, 9, 11, 2, 9, 2, 1,
            3, 3, 10, 1, 3, 11, 10, 7, 8, 4,
            4, 1, 11, 10, 1, 4, 11, 1, 0, 4, 7, 11, 4,
            4, 4, 7, 8, 9, 0, 11, 9, 11, 10, 11, 0, 3,
            3, 4, 7, 11, 4, 11, 9, 9, 11, 10,
            1, 9, 5, 4,
            2, 9, 5, 4, 0, 8, 3,
            2, 0, 5, 4, 1, 5, 0,
            3, 8, 5, 4, 8, 3, 5, 3, 1, 5,
            2, 1, 2, 10, 9, 5, 4,
            3, 3, 0, 8, 1, 2, 10, 4, 9, 5,
            3, 5, 2, 10, 5, 4, 2, 4, 0, 2,
            4, 2, 10, 5, 3, 2, 5, 3, 5, 4, 3, 4, 8,
            2, 9, 5, 4, 2, 3, 11,
            3, 0, 11, 2, 0, 8, 11, 4, 9, 5,
            3, 0, 5, 4, 0, 1, 5, 2, 3, 11,
            4, 2, 1, 5, 2, 5, 8, 2, 8, 11, 4, 8, 5,
            3, 10, 3, 11, 10, 1, 3, 9, 5, 4,
            4, 4, 9, 5, 0, 8, 1, 8, 10, 1, 8, 11, 10,
            4, 5, 4, 0, 5, 0, 11, 5, 11, 10, 11, 0, 3,
            3, 5, 4, 8, 5, 8, 10, 10, 8, 11,
            2, 9, 7, 8, 5, 7, 9,
            3, 9, 3, 0, 9, 5, 3, 5, 7, 3,
            3, 0, 7, 8, 0, 1, 7, 1, 5, 7,
            2, 1, 5, 3, 3, 5, 7,
            3, 9, 7, 8, 9, 5, 7, 10, 1, 2,
            4, 10, 1, 2, 9, 5, 0, 5, 3, 0, 5, 7, 3,
            4, 8, 0, 2, 8, 2, 5, 8, 5, 7, 10, 5, 2,
            3, 2, 10, 5, 2, 5, 3, 3, 5, 7,
            3, 7, 9, 5, 7, 8, 9, 3, 11, 2,
            4, 9, 5, 7, 9, 7, 2, 9, 2, 0, 2, 7, 11,
            4, 2, 3, 11, 0, 1, 8, 1, 7, 8, 1, 5, 7,
            3, 11, 2, 1, 11, 1, 7, 7, 1, 5,
            4, 9, 5, 8, 8, 5, 7, 10, 1, 3, 10, 3, 11,
            5, 5, 7, 0, 5, 0, 9, 7, 11, 0, 1, 0, 10, 11, 10, 0,
            5, 11, 10, 0, 11, 0, 3, 10, 5, 0, 8, 0, 7, 5, 7, 0,
            2, 11, 10, 5, 7, 11, 5,
            1, 10, 6, 5,
            2, 0, 8, 3, 5, 10, 6,
            2, 9, 0, 1, 5, 10, 6,
            3, 1, 8, 3, 1, 9, 8, 5, 10, 6,
            2, 1, 6, 5, 2, 6, 1,
            3, 1, 6, 5, 1, 2, 6, 3, 0, 8,
            3, 9, 6, 5, 9, 0, 6, 0, 2, 6,
            4, 5, 9, 8, 5, 8, 2, 5, 2, 6, 3, 2, 8,
            2, 2, 3, 11, 10, 6, 5,
            3, 11, 0, 8, 11, 2, 0, 10, 6, 5,
            3, 0, 1, 9, 2, 3, 11, 5, 10, 6,
            4, 5, 10, 6, 1, 9, 2, 9, 11, 2, 9, 8, 11,
            3, 6, 3, 11, 6, 5, 3, 5, 1, 3,
            4, 0, 8, 11, 0, 11, 5, 0, 5, 1, 5, 11, 6,
            4, 3, 11, 6, 0, 3, 6, 0, 6, 5, 0, 5, 9,
            3, 6, 5, 9, 6, 9, 11, 11, 9, 8,
            2, 5, 10, 6, 4, 7, 8,
            3, 4, 3, 0, 4, 7, 3, 6, 5, 10,
            3, 1, 9, 0, 5, 10, 6, 8, 4, 7,
            4, 10, 6, 5, 1, 9, 7, 1, 7, 3, 7, 9, 4,
            3, 6, 1, 2, 6, 5, 1, 4, 7, 8,
            4, 1, 2, 5, 5, 2, 6, 3, 0, 4, 3, 4, 7,
            4, 8, 4, 7, 9, 0, 5, 0, 6, 5, 0, 2, 6,
            5, 7, 3, 9, 7, 9, 4, 3, 2, 9, 5, 9, 6, 2, 6, 9,
            3, 3, 11, 2, 7, 8, 4, 10, 6, 5,
            4, 5, 10, 6, 4, 7, 2, 4, 2, 0, 2, 7, 11,
            4, 0, 1, 9, 4, 7, 8, 2, 3, 11, 5, 10, 6,
            5, 9, 2, 1, 9, 11, 2, 9, 4, 11, 7, 11, 4, 5, 10, 6,
            4, 8, 4, 7, 3, 11, 5, 3, 5, 1, 5, 11, 6,
            5, 5, 1, 11, 5, 11, 6, 1, 0, 11, 7, 11, 4, 0, 4, 11,
            5, 0, 5, 9, 0, 6, 5, 0, 3, 6, 11, 6, 3, 8, 4, 7,
            4, 6, 5, 9, 6, 9, 11, 4, 7, 9, 7, 11, 9,
            2, 10, 4, 9, 6, 4, 10,
            3, 4, 10, 6, 4, 9, 10, 0, 8, 3,
            3, 10, 0, 1, 10, 6, 0, 6, 4, 0,
            4, 8, 3, 1, 8, 1, 6, 8, 6, 4, 6, 1, 10,
            3, 1, 4, 9, 1, 2, 4, 2, 6, 4,
            4, 3, 0, 8, 1, 2, 9, 2, 4, 9, 2, 6, 4,
            2, 0, 2, 4, 4, 2, 6,
            3, 8, 3, 2, 8, 2, 4, 4, 2, 6,
            3, 10, 4, 9, 10, 6, 4, 11, 2, 3,
            4, 0, 8, 2, 2, 8, 11, 4, 9, 10, 4, 10, 6,
            4, 3, 11, 2, 0, 1, 6, 0, 6, 4, 6, 1, 10,
            5, 6, 4, 1, 6, 1, 10, 4, 8, 1, 2, 1, 11, 8, 11, 1,
            4, 9, 6, 4, 9, 3, 6, 9, 1, 3, 11, 6, 3,
            5, 8, 11, 1, 8, 1, 0, 11, 6, 1, 9, 1, 4, 6, 4, 1,
            3, 3, 11, 6, 3, 6, 0, 0, 6, 4,
            2, 6, 4, 8, 11, 6, 8,
            3, 7, 10, 6, 7, 8, 10, 8, 9, 10,
            4, 0, 7, 3, 0, 10, 7, 0, 9, 10, 6, 7, 10,
            4, 10, 6, 7, 1, 10, 7, 1, 7, 8, 1, 8, 0,
            3, 10, 6, 7, 10, 7, 1, 1, 7, 3,
            4, 1, 2, 6, 1, 6, 8, 1, 8, 9, 8, 6, 7,
            5, 2, 6, 9, 2, 9, 1, 6, 7, 9, 0, 9, 3, 7, 3, 9,
            3, 7, 8, 0, 7, 0, 6, 6, 0, 2,
            2, 7, 3, 2, 6, 7, 2,
            4, 2, 3, 11, 10, 6, 8, 10, 8, 9, 8, 6, 7,
            5, 2, 0, 7, 2, 7, 11, 0, 9, 7, 6, 7, 10, 9, 10, 7,
            5, 1, 8, 0, 1, 7, 8, 1, 10, 7, 6, 7, 10, 2, 3, 11,
            4, 11, 2, 1, 11, 1, 7, 10, 6, 1, 6, 7, 1,
            5, 8, 9, 6, 8, 6, 7, 9, 1, 6, 11, 6, 3, 1, 3, 6,
            2, 0, 9, 1, 11, 6, 7,
            4, 7, 8, 0, 7, 0, 6, 3, 11, 0, 11, 6, 0,
            1, 7, 11, 6,
            1, 7, 6, 11,
            2, 3, 0, 8, 11, 7, 6,
            2, 0, 1, 9, 11, 7, 6,
            3, 8, 1, 9, 8, 3, 1, 11, 7, 6,
            2, 10, 1, 2, 6, 11, 7,
            3, 1, 2, 10, 3, 0, 8, 6, 11, 7,
            3, 2, 9, 0, 2, 10, 9, 6, 11, 7,
            4, 6, 11, 7, 2, 10, 3, 10, 8, 3, 10, 9, 8,
            2, 7, 2, 3, 6, 2, 7,
            3, 7, 0, 8, 7, 6, 0, 6, 2, 0,
            3, 2, 7, 6, 2, 3, 7, 0, 1, 9,
            4, 1, 6, 2, 1, 8, 6, 1, 9, 8, 8, 7, 6,
            3, 10, 7, 6, 10, 1, 7, 1, 3, 7,
            4, 10, 7, 6, 1, 7, 10, 1, 8, 7, 1, 0, 8,
            4, 0, 3, 7, 0, 7, 10, 0, 10, 9, 6, 10, 7,
            3, 7, 6, 10, 7, 10, 8, 8, 10, 9,
            2, 6, 8, 4, 11, 8, 6,
            3, 3, 6, 11, 3, 0, 6, 0, 4, 6,
            3, 8, 6, 11, 8, 4, 6, 9, 0, 1,
            4, 9, 4, 6, 9, 6, 3, 9, 3, 1, 11, 3, 6,
            3, 6, 8, 4, 6, 11, 8, 2, 10, 1,
            4, 1, 2, 10, 3, 0, 11, 0, 6, 11, 0, 4, 6,
            4, 4, 11, 8, 4, 6, 11, 0, 2, 9, 2, 10, 9,
            5, 10, 9, 3, 10, 3, 2, 9, 4, 3, 11, 3, 6, 4, 6, 3,
            3, 8, 2, 3, 8, 4, 2, 4, 6, 2,
            2, 0, 4, 2, 4, 6, 2,
            4, 1, 9, 0, 2, 3, 4, 2, 4, 6, 4, 3, 8,
            3, 1, 9, 4, 1, 4, 2, 2, 4, 6,
            4, 8, 1, 3, 8, 6, 1, 8, 4, 6, 6, 10, 1,
            3, 10, 1, 0, 10, 0, 6, 6, 0, 4,
            5, 4, 6, 3, 4, 3, 8, 6, 10, 3, 0, 3, 9, 10, 9, 3,
            2, 10, 9, 4, 6, 10, 4,
            2, 4, 9, 5, 7, 6, 11,
            3, 0, 8, 3, 4, 9, 5, 11, 7, 6,
            3, 5, 0, 1, 5, 4, 0, 7, 6, 11,
            4, 11, 7, 6, 8, 3, 4, 3, 5, 4, 3, 1, 5,
            3, 9, 5, 4, 10, 1, 2, 7, 6, 11,
            4, 6, 11, 7, 1, 2, 10, 0, 8, 3, 4, 9, 5,
            4, 7, 6, 11, 5, 4, 10, 4, 2, 10, 4, 0, 2,
            5, 3, 4, 8, 3, 5, 4, 3, 2, 5, 10, 5, 2, 11, 7, 6,
            3, 7, 2, 3, 7, 6, 2, 5, 4, 9,
            4, 9, 5, 4, 0, 8, 6, 0, 6, 2, 6, 8, 7,
            4, 3, 6, 2, 3, 7, 6, 1, 5, 0, 5, 4, 0,
            5, 6, 2, 8, 6, 8, 7, 2, 1, 8, 4, 8, 5, 1, 5, 8,
            4, 9, 5, 4, 10, 1, 6, 1, 7, 6, 1, 3, 7,
            5, 1, 6, 10, 1, 7, 6, 1, 0, 7, 8, 7, 0, 9, 5, 4,
            5, 4, 0, 10, 4, 10, 5, 0, 3, 10, 6, 10, 7, 3, 7, 10,
            4, 7, 6, 10, 7, 10, 8, 5, 4, 10, 4, 8, 10,
            3, 6, 9, 5, 6, 11, 9, 11, 8, 9,
            4, 3, 6, 11, 0, 6, 3, 0, 5, 6, 0, 9, 5,
            4, 0, 11, 8, 0, 5, 11, 0, 1, 5, 5, 6, 11,
            3, 6, 11, 3, 6, 3, 5, 5, 3, 1,
            4, 1, 2, 10, 9, 5, 11, 9, 11, 8, 11, 5, 6,
            5, 0, 11, 3, 0, 6, 11, 0, 9, 6, 5, 6, 9, 1, 2, 10,
            5, 11, 8, 5, 11, 5, 6, 8, 0, 5, 10, 5, 2, 0, 2, 5,
            4, 6, 11, 3, 6, 3, 5, 2, 10, 3, 10, 5, 3,
            4, 5, 8, 9, 5, 2, 8, 5, 6, 2, 3, 8, 2,
            3, 9, 5, 6, 9, 6, 0, 0, 6, 2,
            5, 1, 5, 8, 1, 8, 0, 5, 6, 8, 3, 8, 2, 6, 2, 8,
            2, 1, 5, 6, 2, 1, 6,
            5, 1, 3, 6, 1, 6, 10, 3, 8, 6, 5, 6, 9, 8, 9, 6,
            4, 10, 1, 0, 10, 0, 6, 9, 5, 0, 5, 6, 0,
            2, 0, 3, 8, 5, 6, 10,
            1, 10, 5, 6,
            2, 11, 5, 10, 7, 5, 11,
            3, 11, 5, 10, 11, 7, 5, 8, 3, 0,
            3, 5, 11, 7, 5, 10, 11, 1, 9, 0,
            4, 10, 7, 5, 10, 11, 7, 9, 8, 1, 8, 3, 1,
            3, 11, 1, 2, 11, 7, 1, 7, 5, 1,
            4, 0, 8, 3, 1, 2, 7, 1, 7, 5, 7, 2, 11,
            4, 9, 7, 5, 9, 2, 7, 9, 0, 2, 2, 11, 7,
            5, 7, 5, 2, 7, 2, 11, 5, 9, 2, 3, 2, 8, 9, 8, 2,
            3, 2, 5, 10, 2, 3, 5, 3, 7, 5,
            4, 8, 2, 0, 8, 5, 2, 8, 7, 5, 10, 2, 5,
            4, 9, 0, 1, 5, 10, 3, 5, 3, 7, 3, 10, 2,
            5, 9, 8, 2, 9, 2, 1, 8, 7, 2, 10, 2, 5, 7, 5, 2,
            2, 1, 3, 5, 3, 7, 5,
            3, 0, 8, 7, 0, 7, 1, 1, 7, 5,
            3, 9, 0, 3, 9, 3, 5, 5, 3, 7,
            2, 9, 8, 7, 5, 9, 7,
            3, 5, 8, 4, 5, 10, 8, 10, 11, 8,
            4, 5, 0, 4, 5, 11, 0, 5, 10, 11, 11, 3, 0,
            4, 0, 1, 9, 8, 4, 10, 8, 10, 11, 10, 4, 5,
            5, 10, 11, 4, 10, 4, 5, 11, 3, 4, 9, 4, 1, 3, 1, 4,
            4, 2, 5, 1, 2, 8, 5, 2, 11, 8, 4, 5, 8,
            5, 0, 4, 11, 0, 11, 3, 4, 5, 11, 2, 11, 1, 5, 1, 11,
            5, 0, 2, 5, 0, 5, 9, 2, 11, 5, 4, 5, 8, 11, 8, 5,
            2, 9, 4, 5, 2, 11, 3,
            4, 2, 5, 10, 3, 5, 2, 3, 4, 5, 3, 8, 4,
            3, 5, 10, 2, 5, 2, 4, 4, 2, 0,
            5, 3, 10, 2, 3, 5, 10, 3, 8, 5, 4, 5, 8, 0, 1, 9,
            4, 5, 10, 2, 5, 2, 4, 1, 9, 2, 9, 4, 2,
            3, 8, 4, 5, 8, 5, 3, 3, 5, 1,
            2, 0, 4, 5, 1, 0, 5,
            4, 8, 4, 5, 8, 5, 3, 9, 0, 5, 0, 3, 5,
            1, 9, 4, 5,
            3, 4, 11, 7, 4, 9, 11, 9, 10, 11,
            4, 0, 8, 3, 4, 9, 7, 9, 11, 7, 9, 10, 11,
            4, 1, 10, 11, 1, 11, 4, 1, 4, 0, 7, 4, 11,
            5, 3, 1, 4, 3, 4, 8, 1, 10, 4, 7, 4, 11, 10, 11, 4,
            4, 4, 11, 7, 9, 11, 4, 9, 2, 11, 9, 1, 2,
            5, 9, 7, 4, 9, 11, 7, 9, 1, 11, 2, 11, 1, 0, 8, 3,
            3, 11, 7, 4, 11, 4, 2, 2, 4, 0,
            4, 11, 7, 4, 11, 4, 2, 8, 3, 4, 3, 2, 4,
            4, 2, 9, 10, 2, 7, 9, 2, 3, 7, 7, 4, 9,
            5, 9, 10, 7, 9, 7, 4, 10, 2, 7, 8, 7, 0, 2, 0, 7,
            5, 3, 7, 10, 3, 10, 2, 7, 4, 10, 1, 10, 0, 4, 0, 10,
            2, 1, 10, 2, 8, 7, 4,
            3, 4, 9, 1, 4, 1, 7, 7, 1, 3,
            4, 4, 9, 1, 4, 1, 7, 0, 8, 1, 8, 7, 1,
            2, 4, 0, 3, 7, 4, 3,
            1, 4, 8, 7,
            2, 9, 10, 8, 10, 11, 8,
            3, 3, 0, 9, 3, 9, 11, 11, 9, 10,
            3, 0, 1, 10, 0, 10, 8, 8, 10, 11,
            2, 3, 1, 10, 11, 3, 10,
            3, 1, 2, 11, 1, 11, 9, 9, 11, 8,
            4, 3, 0, 9, 3, 9, 11, 1, 2, 9, 2, 11, 9,
            2, 0, 2, 11, 8, 0, 11,
            1, 3, 2, 11,
            3, 2, 3, 8, 2, 8, 10, 10, 8, 9,
            2, 9, 10, 2, 0, 9, 2,
            4, 2, 3, 8, 2, 8, 10, 0, 1, 8, 1, 10, 8,
            1, 1, 10, 2,
            2, 1, 3, 8, 9, 1, 8,
            1, 0, 9, 1,
            1, 0, 3, 8,
            0
        };

        /// <summary>
        /// A list of indices for accessing <see cref="TriangleTableWithLengths"/>. The nth element in this array tells you the index where the same row starts in <see cref="TriangleTableWithLengths"/>
        /// </summary>
        public static readonly int[] TriangleTableAccessIndices =
        {
            0,
            1,
            5,
            9,
            16,
            20,
            27,
            34,
            44,
            48,
            55,
            62,
            72,
            79,
            89,
            99,
            106,
            110,
            117,
            124,
            134,
            141,
            151,
            161,
            174,
            181,
            191,
            201,
            214,
            224,
            237,
            250,
            260,
            264,
            271,
            278,
            288,
            295,
            305,
            315,
            328,
            335,
            345,
            355,
            368,
            378,
            391,
            404,
            414,
            421,
            431,
            441,
            448,
            458,
            471,
            484,
            494,
            504,
            517,
            530,
            540,
            553,
            569,
            585,
            592,
            596,
            603,
            610,
            620,
            627,
            637,
            647,
            660,
            667,
            677,
            687,
            700,
            710,
            723,
            736,
            746,
            753,
            763,
            773,
            786,
            796,
            809,
            822,
            838,
            848,
            861,
            874,
            890,
            903,
            919,
            935,
            948,
            955,
            965,
            975,
            988,
            998,
            1011,
            1018,
            1028,
            1038,
            1051,
            1064,
            1080,
            1093,
            1109,
            1119,
            1126,
            1136,
            1149,
            1162,
            1172,
            1185,
            1201,
            1211,
            1218,
            1231,
            1247,
            1263,
            1276,
            1292,
            1299,
            1312,
            1316,
            1320,
            1327,
            1334,
            1344,
            1351,
            1361,
            1371,
            1384,
            1391,
            1401,
            1411,
            1424,
            1434,
            1447,
            1460,
            1470,
            1477,
            1487,
            1497,
            1510,
            1520,
            1533,
            1546,
            1562,
            1572,
            1579,
            1592,
            1602,
            1615,
            1625,
            1641,
            1648,
            1655,
            1665,
            1675,
            1688,
            1698,
            1711,
            1724,
            1740,
            1750,
            1763,
            1776,
            1792,
            1805,
            1821,
            1837,
            1850,
            1860,
            1873,
            1886,
            1896,
            1909,
            1925,
            1941,
            1954,
            1967,
            1977,
            1993,
            2000,
            2016,
            2029,
            2036,
            2040,
            2047,
            2057,
            2067,
            2080,
            2090,
            2103,
            2116,
            2132,
            2142,
            2155,
            2168,
            2184,
            2191,
            2201,
            2211,
            2218,
            2228,
            2241,
            2254,
            2270,
            2283,
            2299,
            2315,
            2322,
            2335,
            2345,
            2361,
            2374,
            2384,
            2391,
            2404,
            2408,
            2418,
            2431,
            2444,
            2460,
            2473,
            2489,
            2499,
            2512,
            2525,
            2541,
            2557,
            2564,
            2574,
            2587,
            2594,
            2598,
            2605,
            2615,
            2625,
            2632,
            2642,
            2655,
            2662,
            2666,
            2676,
            2683,
            2696,
            2700,
            2707,
            2711,
            2715
        };
    }
}