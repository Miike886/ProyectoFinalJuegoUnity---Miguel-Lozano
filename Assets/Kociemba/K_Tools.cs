using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

namespace Kociemba
{
    public class Tools
    {
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Comprueba si la cadena de cubo s representa un cubo resoluble.
        // 0: El cubo es resoluble
        // -1: No hay exactamente una faceta de cada color
        // -2: No existen exactamente una vez todas las 12 aristas
        // -3: Error de inversión: Una arista debe estar invertida
        // -4: No existen exactamente una vez todas las 8 esquinas
        // -5: Error de torsión: Una esquina debe estar torcida
        // -6: Error de paridad: Se deben intercambiar dos esquinas o dos aristas
        // 
        /// <summary>
        /// Comprueba si la cadena de definición del cubo s representa un cubo resoluble.
        /// </summary>
        /// <param name="s"> es la cadena de definición del cubo, ver <seealso cref="Facelet"/> </param>
        /// <returns> 0: El cubo es resoluble<br>
        ///         -1: No hay exactamente una faceta de cada color<br>
        ///         -2: No existen exactamente una vez todas las 12 aristas<br>
        ///         -3: Error de inversión: Una arista debe estar invertida<br>
        ///         -4: No existen exactamente una vez todas las 8 esquinas<br>
        ///         -5: Error de torsión: Una esquina debe estar torcida<br>
        ///         -6: Error de paridad: Se deben intercambiar dos esquinas o dos aristas </returns>
        public static int verify(string s)
        {
            int[] count = new int[6];
            try
            {
                for (int i = 0; i < 54; i++)
                {
                    count[(int)CubeColor.Parse(typeof(CubeColor), i.ToString())]++;
                }
            }
            catch (Exception)
            {
                return -1;
            }

            for (int i = 0; i < 6; i++)
            {
                if (count[i] != 9)
                {
                    return -1;
                }
            }

            FaceCube fc = new FaceCube(s);
            CubieCube cc = fc.toCubieCube();

            return cc.verify();
        }

        /// <summary>
        /// Genera un cubo aleatorio. </summary>
        /// <returns> Un cubo aleatorio en representación de cadena. Cada cubo del espacio de cubos tiene la misma probabilidad. </returns>
        public static string randomCube()
        {
            CubieCube cc = new CubieCube();
            System.Random gen = new System.Random();
            cc.setFlip((short)gen.Next(CoordCube.N_FLIP));
            cc.setTwist((short)gen.Next(CoordCube.N_TWIST));
            do
            {
                cc.setURFtoDLB(gen.Next(CoordCube.N_URFtoDLB));
                cc.setURtoBR(gen.Next(CoordCube.N_URtoBR));
            } while ((cc.edgeParity() ^ cc.cornerParity()) != 0);
            FaceCube fc = cc.toFaceCube();
            return fc.to_fc_String();
        }


        // https://stackoverflow.com/questions/7742519/c-sharp-export-write-multidimension-array-to-file-csv-or-whatever
        // Kristian Fenn: https://stackoverflow.com/users/989539/kristian-fenn

        public static void SerializeTable(string filename, short[,] array)
        {
            EnsureFolder("Assets\\Resources\\");
            BinaryFormatter bf = new BinaryFormatter();
            Stream s = File.Open("Assets\\Resources\\" + filename + ".bytes", FileMode.Create);
            bf.Serialize(s, array);
            s.Close();
        }

        public static short[,] DeserializeTable(string filename)
        {
            
            TextAsset asset = Resources.Load(filename) as TextAsset;
            Stream s = new MemoryStream(asset.bytes);
            //Stream s = File.Open("Assets\\Resources\\" + filename, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            short[,] array = (short[,])bf.Deserialize(s);
            s.Close();
            return array;
        }

        public static void SerializeSbyteArray(string filename, sbyte[] array)
        {
            EnsureFolder("Assets\\Resources\\");
            BinaryFormatter bf = new BinaryFormatter();
            Stream s = File.Open("Assets\\Resources\\" + filename + ".bytes", FileMode.Create);
            bf.Serialize(s, array);
            s.Close();
        }

        public static sbyte[] DeserializeSbyteArray(string filename)
        {
            TextAsset asset = Resources.Load(filename) as TextAsset;
            Stream s = new MemoryStream(asset.bytes);
            //Stream s = File.Open("Assets\\Resources\\" + filename, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            sbyte[] array = (sbyte[])bf.Deserialize(s);
            s.Close();
            return array;
        }

        // https://stackoverflow.com/questions/3695163/filestream-and-creating-folders
        // Joe: https://stackoverflow.com/users/13087/joe

        static void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            // If path is a file name only, directory name will be an empty string
            if (directoryName.Length > 0)
            {
                // Create all directories on the path that don't already exist
                Directory.CreateDirectory(directoryName);
            }
        }
    }    
}
