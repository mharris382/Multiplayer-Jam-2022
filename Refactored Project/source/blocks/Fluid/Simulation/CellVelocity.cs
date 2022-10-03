using System;
using Game.Blocks.Gas;
using Godot;

namespace Game.Blocks.Fluid
{
    [Flags]
    public enum CellVelocity
    {
        X_IS_1 = 1 << 0,
        Y_IS_1 = 1 << 1,
        
        X_IS_POS = 1 << 3,
        Y_IS_POS = 1 << 4,
        
        X_IS_NEG = X_IS_1,
        Y_IS_NEG = Y_IS_1,
        
        
        DIR_R = X_IS_POS ,
        DIR_R_U = X_IS_POS | Y_IS_POS,
        DIR_U = Y_IS_POS,
        DIR_L_U = X_IS_NEG | Y_IS_POS,
        DIR_L = X_IS_NEG ,
        DIR_L_D = X_IS_NEG | Y_IS_NEG,
        DIR_D =  Y_IS_NEG,
        DIR_R_D = X_IS_POS | Y_IS_NEG,
        
        ZERO = 0,
        MASK_X = (X_IS_1 | X_IS_POS)
    }
    
    
    public static class EnumVelocityExtensions
    {
         public static CellVelocity GetX(this CellVelocity cellVelocity)
         {
             return (CellVelocity)((byte)cellVelocity & (byte)(CellVelocity.MASK_X));
         }
         public static CellVelocity GetY(this CellVelocity cellVelocity)
         {
             return (CellVelocity)((byte)cellVelocity & (byte)(CellVelocity.MASK_X));
         }

         const int UP_SIGN = 1;
         
         public static Vector2Int Z => Vector2Int.Zero;
         public static Vector2Int R =>Vector2Int.R ;
         public static Vector2Int RU =>Vector2Int.RU;
         public static Vector2Int U =>Vector2Int.U ;
         public static Vector2Int LU =>Vector2Int.LU;
         public static Vector2Int L =>Vector2Int.L ;
         public static Vector2Int LD =>Vector2Int.LD;
         public static Vector2Int D =>Vector2Int.D ;
         public static Vector2Int RD =>Vector2Int.RD;
         
         
         
         public static Vector2Int ToVec(this CellVelocity cellVelocity)
         {
             switch (cellVelocity)
             {
                 case CellVelocity.DIR_R_U:
                     return RU;
                 case CellVelocity.DIR_L_U:
                     return LU;
                 case CellVelocity.DIR_L_D:
                     return LD;
                 case CellVelocity.DIR_R_D:
                     return RD;
                 case CellVelocity.DIR_D:
                     return D;
                 case CellVelocity.DIR_U:
                     return U;
                 case CellVelocity.DIR_R:
                     return R;
                 case CellVelocity.DIR_L:
                     return L;
                 case CellVelocity.ZERO:
                     return Z;
                 default:
                     throw new ArgumentOutOfRangeException(nameof(cellVelocity), cellVelocity, null);
             }

         }

         public static CellVelocity FromVec(this Vector2Int x)
         {
             var res = 0;
             switch (Mathf.Sign(x.x))
             {
                 case 1:
                     res |= (int)CellVelocity.DIR_R;
                     break;
                 case -1:
                     res |= (int)CellVelocity.DIR_L;
                     break;
             }
             switch (Mathf.Sign(x.y))
             {
                 case 1:
                     res |= (int)CellVelocity.DIR_U;
                     break;
                 case -1:
                     res |= (int)CellVelocity.DIR_D;
                     break;
             }
             return (CellVelocity)res;
         }
         
         public static bool HasX(this CellVelocity cellVelocity)
         {
             return ((byte)cellVelocity & ((byte)CellVelocity.X_IS_1)) != 0;
         }
         
         public static bool HasY(this CellVelocity cellVelocity)
         {
             return ((byte)cellVelocity & ((byte)CellVelocity.Y_IS_1)) != 0;
         }

         private static CellVelocity SetX(this CellVelocity cellVelocity, int value)
         {
             var b =(byte)cellVelocity;
             if (value == 0)
             {
                 b |= (byte)CellVelocity.X_IS_1;
             }
             else if(value < 0)
             {
                 b |= (byte)CellVelocity.X_IS_NEG;
             }
             else
             {
                 b |= (byte)CellVelocity.X_IS_POS;
             }
             return (CellVelocity)b;
         }
     }
     
}