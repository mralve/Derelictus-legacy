/* Copyright (C) Xonomoto Studios - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Alve Larsson <zurra.alve@gmail.com>, September 2018
 */

using UnityEngine;
using System;
using System.IO;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConstruiSystem
{
    public static class XCPManager
    {
        public static XCP currentXCP;

        public static string importPath;

        public static bool XCPExportToFile(XCP fileData, string filePath = null)
        {
            /*
            if(XCPManager.currentXCP != null){
                currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex] = XCPManager.currentXCP.xpcMaps[MapDataManager.mapDataXCPIndex];
            }
             */
            using (Stream curStream = File.Create(filePath))
            {
                BinaryFormatter curBinaryFormater = new BinaryFormatter();
                curBinaryFormater.Serialize(curStream, fileData);
                curStream.Close();
            }
            return true;
        }

        // Load a file from the users OS and extract the mapdata from the file.
        public static XCP XCPImportFromFile(string filePath = null)
        {

            if (File.Exists(filePath))
            {
                FileStream stream = new FileStream(filePath, FileMode.Open);
                BinaryFormatter curBinaryFormater = new BinaryFormatter();

                XCP fileData = curBinaryFormater.Deserialize(stream) as XCP;
                stream.Close();

                importPath = filePath;
                currentXCP = fileData;
                return fileData;
            }
            else
            {
                return null;
            }
        }

        public static Sprite TexToSprite(Texture2D tex, float sortingPoint = 0.5f)
        {
            return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }

        public static Png TexToPng(Texture2D tex)
        {
            Png img = new Png();
            img.pixScale = 1;
            img.t = ImageConversion.EncodeToPNG(tex);
            return img;
        }
        public static Texture2D PngToTex(Png png)
        {
            Texture2D img = new Texture2D(0, 0);
            img.LoadImage(png.t);
            img.filterMode = FilterMode.Point;
            return img;
        }

        public static Sprite PngToSprite(Png tex)
        {
            Texture2D img = new Texture2D(1, 1);
            img.LoadImage(tex.t);
            img.filterMode = FilterMode.Point;
            return TexToSprite(img, tex.sortPoint);
        }

        public static Png[] SpritesToPngs( Sprite[] sprites )
        {
            if(sprites.Length != 0)
            {
                Png[] pngs = new Png[sprites.Length];
                for (int i = 0; i < sprites.Length; i++)
                {
                    pngs[i] = TexToPng( sprites[i].texture );
                }
                return pngs;
            }
            return null;
        }
    }
}