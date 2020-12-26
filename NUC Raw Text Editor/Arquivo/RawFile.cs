using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using NUC_Raw_Tools.Arquivo;

namespace NUC_Raw_Tools
{
    public  class RawFile
    {

        #region RAW FILE STRUCT

        //        Estrutura dos arquivos RAW do CFC.DIG:

        //| Offset |  TIPO  | Valor
        //|--------|--------|---------------------------
        //|   0    | Uint32 | Número de arquivo(id)
        //|   4    | Uint32 | Tamanho do arquivo
        //|   8    | Uint32 | Offset inicial do arquivo
        //|   C    | Uint32 | <END> (4) null bytes

        //A estrutura é mostrada mesmo nos arquivos RAW dentro de si mesmos.
        //E é logo no início do arquivo, no offset 0.

        //Por ser Uint32 os 4 bytes são lidos ao reverso!

        //Logicamente, a tabela em questão(desses ponteiros iniciais), 
        //termina no offset do primeiro arquivo, pois o primeiro arquivo(De id = 0) 
        //é o que dá início logo depois da tabela de ponteiros.

        #endregion


        public static RawFile ReadBlock(byte[] raw)
        {
            RawFile rf = null;
            
            return rf;
        }
        public void Rebuild()
        {
            
        }

        public void Save(string filename)
        {
            Rebuild();
        
        }
        public static UInt32 ReadUInt32(Stream s, uint offset)
        {
            //byte[] buff = new byte[4];
            var rr = new BinaryReader(s);
            rr.BaseStream.Position = offset;
            return rr.ReadUInt32();
            //return BitConverter.ToUInt32(buff, 0);
        }

        public static void WriteUInt32(Stream s, uint u)
        {
            s.Write(BitConverter.GetBytes(u), 0, 4);
        }

    }
}
