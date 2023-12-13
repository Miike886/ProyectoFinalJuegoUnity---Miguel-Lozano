using System;

namespace Kociemba
{
    internal class CoordCube
    {
        // Números de coordenadas para diferentes movimientos
        internal const short N_TWIST = 2187; // 3^7 posibles orientaciones de las esquinas
        internal const short N_FLIP = 2048; // 2^11 posibles flips de los bordes
        internal const short N_SLICE1 = 495; // 12 elegir 4 posiciones posibles de los bordes FR, FL, BL, BR
        internal const short N_SLICE2 = 24; // 4! permutaciones de los bordes FR, FL, BL, BR en fase 2
        internal const short N_PARITY = 2; // 2 paridades posibles de las esquinas
        internal const short N_URFtoDLF = 20160; // 8!/(8-6)! permutación de las esquinas URF, UFL, ULB, UBR, DFR, DLF
        internal const short N_FRtoBR = 11880; // 12!/(12-4)! permutación de los bordes FR, FL, BL, BR
        internal const short N_URtoUL = 1320; // 12!/(12-3)! permutación de los bordes UR, UF, UL
        internal const short N_UBtoDF = 1320; // 12!/(12-3)! permutación de los bordes UB, DR, DF
        internal const short N_URtoDF = 20160; // 8!/(8-6)! permutación de los bordes UR, UF, UL, UB, DR, DF en fase 2

        internal const int N_URFtoDLB = 40320; // 8! permutaciones de las esquinas
        internal const int N_URtoBR = 479001600; // 8! permutaciones de las esquinas

        internal const short N_MOVE = 18;

        // Todas las coordenadas son 0 para un cubo resuelto excepto UBtoDF, que es 114
        internal short twist;
        internal short flip;
        internal short parity;
        internal short FRtoBR;
        internal short URFtoDLF;
        internal short URtoUL;
        internal short UBtoDF;
        internal int URtoDF;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Generar un CoordCube a partir de un CubieCube
        internal CoordCube(CubieCube c, DateTime startTime, string currentTime, out string info)
        {
            info = currentTime;
            twist = c.getTwist();

            flip = c.getFlip();
            parity = c.cornerParity();
            FRtoBR = c.getFRtoBR();

            URFtoDLF = c.getURFtoDLF();
            URtoUL = c.getURtoUL();
            UBtoDF = c.getUBtoDF();
            URtoDF = c.getURtoDF(); // solo necesario en la fase 2
            info += "[ Inicialización finalizada: " + String.Format(@"{0:mm\:ss\.ffff}", (DateTime.Now - startTime)) + " ] ";

        }

        // Un movimiento a nivel de coordenadas
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        internal virtual void move(int m)
        {
            twist = twistMove[twist, m];
            flip = flipMove[flip, m];
            parity = parityMove[parity][m];
            FRtoBR = FRtoBR_Move[FRtoBR, m];
            URFtoDLF = URFtoDLF_Move[URFtoDLF, m];
            URtoUL = URtoUL_Move[URtoUL, m];
            UBtoDF = UBtoDF_Move[UBtoDF, m];
            if (URtoUL < 336 && UBtoDF < 336) // actualizado solo si UR, UF, UL, UB, DR, DF
            {
                // no están en la UD-slice
                URtoDF = MergeURtoULandUBtoDF[URtoUL, UBtoDF];
            }
        }


        // ******************************************Tablas de movimientos de la Fase 1*****************************************************

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Tabla de movimientos para las torsiones de las esquinas
        // twist < 2187 en la fase 2.
        // twist = 0 en la fase 2.
        internal static short[,] twistMove = CoordCubeTables.twist;
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Tabla de movimientos para los flips de los bordes
        // flip < 2048 en la fase 1
        // flip = 0 en la fase 2.
        internal static short[,] flipMove = CoordCubeTables.flip;
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Paridad de la permutación de la esquina. Esto es lo mismo que la paridad para la permutación del borde de un cubo válido.
        // parity tiene valores 0 y 1
        internal static short[][] parityMove = new short[][]
        {
        new short[] {1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1},
        new short[] {0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0}
        };


        // ***********************************Tabla de movimientos de la Fase 1 y 2********************************************************

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Tabla de movimientos para los cuatro bordes de la UD-slice FR, FL, Bl y BR
        // FRtoBRMove < 11880 en la fase 1
        // FRtoBRMove < 24 en la fase 2
        // FRtoBRMove = 0 para el cubo resuelto
        internal static short[,] FRtoBR_Move = CoordCubeTables.FRtoBR;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Tabla de movimientos para la permutación de seis esquinas. Las posiciones de las esquinas DBL y DRB están determinadas por la paridad.
        // URFtoDLF < 20160 en la fase 1
        // URFtoDLF < 20160 en la fase 2
        // URFtoDLF = 0 para el cubo resuelto.
        internal static short[,] URFtoDLF_Move = CoordCubeTables.URFtoDLF;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Tabla de movimientos para la permutación de seis bordes en la cara U y D en la fase2. Las posiciones de los bordes DL y DB están
        // determinadas por la paridad.
        // URtoDF < 665280 en la fase 1
        // URtoDF < 20160 en la fase 2
        // URtoDF = 0 para el cubo resuelto.
        internal static short[,] URtoDF_Move = CoordCubeTables.URtoDF;

        // **************************tablas de movimientos auxiliares para calcular URtoDF al principio de la fase 2************************

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Tabla de movimientos para los tres bordes UR, UF y UL en la fase1.
        internal static short[,] URtoUL_Move = CoordCubeTables.URtoUL;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Tabla de movimientos para los tres bordes UB, DR y DF en la fase1.
        internal static short[,] UBtoDF_Move = CoordCubeTables.UBtoDF;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Tabla para fusionar las coordenadas de los bordes UR, UF, UL y UB, DR, DF al principio de la fase2
        internal static short[,] MergeURtoULandUBtoDF = CoordCubeTables.MergeURtoULandUBtoDF;


        // ****************************************Tablas de poda para la búsqueda*********************************************

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Tabla de poda para la permutación de las esquinas y los bordes de la UD-slice en la fase2.
        // Las entradas de la tabla de poda dan una estimación inferior para el número de movimientos para llegar al cubo resuelto.
        internal static sbyte[] Slice_URFtoDLF_Parity_Prun = CoordCubeTables.Slice_URFtoDLF_Parity_Prun;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Tabla de poda para la permutación de los bordes en la fase2.
        // Las entradas de la tabla de poda dan una estimación inferior para el número de movimientos para llegar al cubo resuelto.
        internal static sbyte[] Slice_URtoDF_Parity_Prun = CoordCubeTables.Slice_URtoDF_Parity_Prun;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Tabla de poda para la torsión de las esquinas y la posición (no permutación) de los bordes de la UD-slice en la fase1
        // Las entradas de la tabla de poda dan una estimación inferior para el número de movimientos para llegar al subgrupo H.
        internal static sbyte[] Slice_Twist_Prun = CoordCubeTables.Slice_Twist_Prun;

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Tabla de poda para el flip de los bordes y la posición (no permutación) de los bordes de la UD-slice en la fase1
        // Las entradas de la tabla de poda dan una estimación inferior para el número de movimientos para llegar al subgrupo H.
        internal static sbyte[] Slice_Flip_Prun = CoordCubeTables.Slice_Flip_Prun;
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

 
        // Establecer el valor de poda en la tabla. Se almacenan dos valores en un byte.
        internal static void setPruning(sbyte[] table, int index, sbyte value)
        {
            if ((index & 1) == 0)
            {
                table[index / 2] &= unchecked((sbyte)(0xf0 | value));
            }
            else
            {
                table[index / 2] &= (sbyte)(0x0f | (value << 4));
            }
        }

        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Extraer el valor de poda
        internal static sbyte getPruning(sbyte[] table, int index)
        {
            if ((index & 1) == 0)
            {
                return (sbyte)(table[index / 2] & 0x0f);
            }
            else
            {
                return (sbyte)((int)((uint)(table[index / 2] & 0xf0) >> 4));
            }
        }
    }

    public static class CoordCubeTables
    {
        
        // Movimiento
        public static readonly short[,] twist = Tools.DeserializeTable("twist");
        public static readonly short[,] flip = Tools.DeserializeTable("flip");
        public static readonly short[,] FRtoBR = Tools.DeserializeTable("FRtoBR");
        public static readonly short[,] URFtoDLF = Tools.DeserializeTable("URFtoDLF");
        public static readonly short[,] URtoDF = Tools.DeserializeTable("URtoDF");
        public static readonly short[,] URtoUL = Tools.DeserializeTable("URtoUL");
        public static readonly short[,] UBtoDF = Tools.DeserializeTable("UBtoDF");
        public static readonly short[,] MergeURtoULandUBtoDF = Tools.DeserializeTable("MergeURtoULandUBtoDF");        

        //Poda
        public static readonly sbyte[] Slice_URFtoDLF_Parity_Prun = Tools.DeserializeSbyteArray("Slice_URFtoDLF_Parity_Prun");
        public static readonly sbyte[] Slice_URtoDF_Parity_Prun = Tools.DeserializeSbyteArray("Slice_URtoDF_Parity_Prun");
        public static readonly sbyte[] Slice_Twist_Prun = Tools.DeserializeSbyteArray("Slice_Twist_Prun");
        public static readonly sbyte[] Slice_Flip_Prun = Tools.DeserializeSbyteArray("Slice_Flip_Prun");
        
    }
    

}
