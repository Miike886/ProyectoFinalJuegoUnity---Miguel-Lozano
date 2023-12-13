namespace Kociemba
{
    /**
     * <pre>
     * Los nombres de las posiciones de las pegatinas en la cara del cubo
     *             |************|
     *             |*U1**U2**U3*|
     *             |************|
     *             |*U4**U5**U6*|
     *             |************|
     *             |*U7**U8**U9*|
     *             |************|
     * ************|************|************|************|
     * *L1**L2**L3*|*F1**F2**F3*|*R1**R2**F3*|*B1**B2**B3*|
     * ************|************|************|************|
     * *L4**L5**L6*|*F4**F5**F6*|*R4**R5**R6*|*B4**B5**B6*|
     * ************|************|************|************|
     * *L7**L8**L9*|*F7**F8**F9*|*R7**R8**R9*|*B7**B8**B9*|
     * ************|************|************|************|
     *             |************|
     *             |*D1**D2**D3*|
     *             |************|
     *             |*D4**D5**D6*|
     *             |************|
     *             |*D7**D8**D9*|
     *             |************|
     * </pre>
     * 
     * Una cadena de definición de cubo "UBL..." significa, por ejemplo: En la posición U1 tenemos el color U, en la posición U2 tenemos el
     * color B, en la posición U3 tenemos el color L, etc., según el orden U1, U2, U3, U4, U5, U6, U7, U8, U9, R1, R2,
     * R3, R4, R5, R6, R7, R8, R9, F1, F2, F3, F4, F5, F6, F7, F8, F9, D1, D2, D3, D4, D5, D6, D7, D8, D9, L1, L2, L3, L4,
     * L5, L6, L7, L8, L9, B1, B2, B3, B4, B5, B6, B7, B8, B9 de las constantes de enumeración.
     */
    public enum Facelet
    {
        U1, U2, U3, U4, U5, U6, U7, U8, U9, R1, R2, R3, R4, R5, R6, R7, R8, R9, F1, F2, F3, F4, F5, F6, F7, F8, F9, D1, D2, D3, D4, D5, D6, D7, D8, D9, L1, L2, L3, L4, L5, L6, L7, L8, L9, B1, B2, B3, B4, B5, B6, B7, B8, B9
    }

    //++++++++++++++++++++++++++++++ Nombres de los colores de las pegatinas del cubo ++++++++++++++++++++++++++++++++++++++++++++++++
    public enum CubeColor
    {
        U, R, F, D, L, B
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //Los nombres de las posiciones de las esquinas del cubo. La esquina URF, por ejemplo, tiene pegatinas U(p), R(ight) y F(ront)
    public enum Corner
    {
        URF, UFL, ULB, UBR, DFR, DLF, DBL, DRB
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //Los nombres de las posiciones de los bordes del cubo. El borde UR, por ejemplo, tiene pegatinas U(p) y R(ight).
    public enum Edge
    {
        UR, UF, UL, UB, DR, DF, DL, DB, FR, FL, BL, BR
    }
}