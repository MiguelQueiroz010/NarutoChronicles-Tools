using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace NUC_Raw_Tools
{
    public class Encodings
    {
        public class Naruto
        {
            public class UzumakiChronicles2
            {

                public static byte[] GetBytes(string s)
                {
                    var seq = new List<byte>();
                    
                    return seq.ToArray();
                }
                public static string GetString(byte[] bytes)
                {
                    var llv = new LetraseValores(File.ReadAllText("Encoding.enc", Encoding.Default));
                    string seq = "";
                    for (int i =0;i<bytes.Length;i+=2)
                    {
                        for (int j = 0; j < llv.vals.Count; j++)
                        {

                            if (bytes[i].ToString("X2") + bytes[i + 1].ToString("X2") == llv.vals[j])
                            {

                                seq += llv.words[j];

                            }

                        }
                         if (bytes[i].ToString("X2") + bytes[i + 1].ToString("X2") == "0180")
                        {
                            seq += "\n";
                        }
                    }
                    return seq;
                }

            }

        }

    }
}
