using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace KBOLib.Util
{
    public class SVCFileUpLoad
    {
        /// <summary>
        /// 저장할 경로 변수
        /// </summary>
        //private string saveDir = "D:\\WEBDIR\\new.kgagolf.or.kr\\User_File";
        private string saveDir = "";

        public void SvcFileUpload()
        {

            //디자인된 구성 요소를 사용하는 경우 다음 줄의 주석 처리를 제거합니다. 
            //InitializeComponent(); 

        }


        /// <summary>
        /// 저장 경로 반환
        /// </summary>
        /// <returns></returns>
        public string ReturnDirectory()
        {
            return saveDir;
        }

        /// <summary>
        /// 저장 경로 
        /// </summary>
        /// <param name="saveDir"></param>
        public void SetSaveDir(String _saveDir)
        {
            saveDir = _saveDir;
        }

        /// <summary>
        /// 디렉토리를 생성
        /// </summary>
        /// <param name="dir"></param>
        public bool CreateDirectory(string dir)
        {
            bool createDir = false;
            string strDir = string.Empty;
            strDir = ReturnDirectory() + "\\" + dir;

            //디렉토리 생성
            if (!Directory.Exists(strDir))
            {
                Directory.CreateDirectory(strDir);

            }
            createDir = true;

            return createDir;

        }

        /// <summary>
        /// 파일 여부 체크
        /// </summary>
        /// <param name="fname">저장폴더와 함께 파일명</param>
        /// <returns></returns>
        public bool EqualsFile(string fname)
        {
            string strFile = ReturnDirectory() + "\\" + fname;
            return File.Exists(strFile);
        }

        /// <summary>
        /// 새로운 파일명 생성
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string GetAvailablePathname(string folderPath, string filename)
        {
            int invalidChar = 0;
            do
            {
                // 파일명에서 사용할 수 없는 문자가 들어 있는 배열을 가져온다.
                invalidChar = filename.IndexOfAny(Path.GetInvalidFileNameChars());

                // 사용할 수 없는 문자 제거
                if (invalidChar != -1)
                    filename = filename.Remove(invalidChar, 1);
            }
            while (invalidChar != -1);

            string fullPath = Path.Combine(folderPath, filename);
            string filenameWithoutExtention = Path.GetFileNameWithoutExtension(filename);
            string extension = Path.GetExtension(filename);

            while (File.Exists(fullPath))
            {
                Regex rg = new Regex(@".*\((?<Num>\d*)\)");
                Match mt = rg.Match(fullPath);

                if (mt.Success)
                {
                    string numberOfCopy = mt.Groups["Num"].Value;
                    int nextNumberOfCopy = int.Parse(numberOfCopy) + 1;
                    int posStart = fullPath.LastIndexOf("(" + numberOfCopy + ")");
                    fullPath = string.Format("{0}({1}){2}", fullPath.Substring(0, posStart), nextNumberOfCopy, extension);
                }
                else
                {
                    fullPath = folderPath + filenameWithoutExtention + " (2)" + extension;
                }
            }
            return fullPath;
        }


        /// <summary>
        /// 파일을 저장 - 파일이 있을 경우 같은 이름으로 저장
        /// </summary>
        /// <param name="httpFile"></param>
        /// <param name="dir"></param>
        /// <param name="id"></param>
        /// <param name="kno"></param>
        /// <returns></returns>
        public string SameFileSaveFile(byte[] fileInfo, string dir, string fileName)
        {
            string checkSave = string.Empty;
            string filePath = string.Empty;


            // 저장할 경로
            string dirpath = ReturnDirectory() + "\\" + dir + "\\";
            filePath = dirpath + fileName;
            if (CreateDirectory(dir))
            {
                try
                {
                    //파일처리
                    using (MemoryStream ms = new MemoryStream(fileInfo))
                    {
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            ms.WriteTo(fs);
                            ms.Close();
                            fs.Close();
                        }
                    }
                    checkSave = fileName; // 파일명
                }
                catch
                {
                    checkSave = "";
                }
            }
            else
            {
                checkSave = "";
            }
            return checkSave;
        }


        /// <summary>
        /// 파일을 저장 - 파일이 있을 경우 새이름으로
        /// </summary>
        /// <param name="httpFile"></param>
        /// <param name="dir"></param>
        /// <param name="id"></param>
        /// <param name="kno"></param>
        /// <returns></returns>
        public string NewFileSaveFile(byte[] fileInfo, string dir, string fileName)
        {
            string checkSave = string.Empty;
            string filePath = string.Empty;

            // 저장할 경로
            string dirpath = ReturnDirectory() + "\\" + dir + "\\";
            if (CreateDirectory(dir))
            {
                try
                {
                    // 중복되지 않는 파일명 생성
                    filePath = GetAvailablePathname(dirpath, fileName);

                    //파일처리
                    using (MemoryStream ms = new MemoryStream(fileInfo))
                    {
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            ms.WriteTo(fs);
                            ms.Close();
                            fs.Close();
                        }
                    }
                    int fnLen = filePath.Length;
                    int fnStart = filePath.LastIndexOf('\\') + 1;
                    checkSave = filePath.Substring(fnStart, fnLen - fnStart); // 파일명
                }
                catch
                {
                    checkSave = "";
                }
            }
            else
            {
                checkSave = "";
            }
            return checkSave;
        }

        /// <summary>
        /// 파일을 삭제한다.
        /// </summary>
        /// <param name="DirPath">디렉토리 절대경로</param>
        /// <param name="FileName">파일명</param>
        /// <returns></returns>
        public bool DeleteFile(string dir, string filename)
        {
            bool checkDel = false;

            try
            {
                string strDir = string.Empty;
                strDir = ReturnDirectory() + "\\" + dir + "\\";

                //파일삭제
                if (File.Exists(strDir + filename))
                {
                    File.Delete(strDir + filename);
                }
                checkDel = true;
            }
            catch
            {
                checkDel = false;
            }

            return checkDel;
        }
    }
}