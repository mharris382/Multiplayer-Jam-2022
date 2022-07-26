﻿using System;
using Godot;

namespace Game.blocks.gas
{
    public struct GasParticle
    {
        public Vector2 Cell { get; set; }
        
        /// <summary>
        /// discrete representation of air density.  air value ranges between 1-16.  
        /// <para>
        /// 0 is a special value representing no air.
        /// 1 is the minimum amount of air that fits into a cell.  At this value the air is considered to have no pressure.
        /// 16 is the maximum air which is allowed in a cell.  This is the maximum air pressure allowed in the cell.
        /// </para>
        /// </summary>
        public int AirDensity { get; private set; }
        
        private GasParticle(int x, int y)
        {
            Cell = new Vector2(x, y);
            AirDensity = 1;
        }
        
        
    }

    public struct GridCell
    {
        public GridCell(Vector2Int position, GridResolution resolution = GridResolution.PX128)
        {
            Position = position;
            Resolution = resolution;
        }

        private Vector2Int Position { get; set; }
        private GridResolution Resolution { get; }


        
        /// <summary>
        /// translates a cell from one resolution to a different resolution.  NOTE: there will be a loss of information
        /// when remapping a smaller resolution to a larger resolution (i.e. PX32 to PX128).  When translating the
        /// other direction, the top left (i.e. smallest x,y) coordinate will be returned that overlaps the larger coord
        /// </summary>
        /// <param name="originalCell">the cell which will have it's position remapped to the target resolution </param>
        /// <param name="targetResolution">the resolution that the output cell will have</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static GridCell RemapToGrid(GridCell originalCell, GridResolution targetResolution)
        {

            throw new NotImplementedException();
        }
    }

    public enum GridResolution
    {
        PX128 = 1,
        PX64 = 2,
        PX32 = 4
    }
}