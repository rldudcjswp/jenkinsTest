using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace KBOLib.Util
{
    public class FileUpLoad
    {
        #region 변수



        /// <summary>
        /// 최대 업로드 용량
        /// </summary>
        //private int maxLength = 102400 * 10; // 5MB
        //private int maxLength = 419430 * 10; // 4MB
        private int maxLength = 629145 * 10; // 4MB

        /// <summary>
        /// 최대 업로드 용량
        /// </summary>
        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }

        /// <summary>
        /// 파일명
        /// </summary>
        public string filename = string.Empty;

        public string Getfilename()
        {
            return filename;
        }

        /// <summary>
        /// 지원 확장자
        /// </summary>
        //private string[] ext = {"GIF", "PNG", "JPG", "JEPG", "BMP" };
        public List<string> listExt = new List<string>();

        /// <summary>
        /// 상태 변수
        /// READY : 준비
        /// NOEXT : 지원되지 않는 확장자
        /// NOFNM : 잘못된 파일명
        /// FAILD : 서버에서 실패
        /// SUCES : 성공
        /// NOSEL : 파일선택안함
        /// </summary>
        private string RTN_STATUS = string.Empty;

        #endregion

        #region 생성자

        /// <summary>
        /// 생성자 
        /// </summary>
        public void SiteUpload()
        {
            // 지원되는 파일 확장자
            listExt.Add("GIF");
            listExt.Add("PNG");
            listExt.Add("JPG");
            listExt.Add("JPEG"); // 2017-05-22 yeeun, 홍지희 사원 요청, JEPG -> JPEG
            listExt.Add("BMP");
            listExt.Add("HWP");
            listExt.Add("DOC");
            listExt.Add("DOCX");
            listExt.Add("PDF");
            listExt.Add("PPT");
            listExt.Add("PPTX");
            listExt.Add("XLSX");
            listExt.Add("XLS");
            listExt.Add("MP4");
            listExt.Add("AVI");
            listExt.Add("ZIP");
            // 20150317 ai 추가
            listExt.Add("AI");
            // 초기 상태
            RTN_STATUS = "READY";
        }

        /// <summary>
        /// 생성자 - 이미지
        /// </summary>
        public void ImgExtUpload()
        {
            // 지원되는 파일 확장자
            listExt.Add("GIF");
            listExt.Add("PNG");
            listExt.Add("JPG");
            listExt.Add("JPEG"); // 2017-05-22 yeeun, 홍지희 사원 요청, JEPG -> JPEG

            // 초기 상태
            RTN_STATUS = "READY";
        }

        /// <summary>
        /// 저장할 경로 변수
        /// </summary>
        //private string saveDir = "D:\\WEBDIR\\new.kgagolf.or.kr\\User_File";
        private string saveDir = "";

        public void SetSaveDir(String _saveDir)
        {
            saveDir = _saveDir;
        }

        #endregion

        #region 디렉토리 생성
        /// <summary>
        /// 디렉토리 생성
        /// </summary>
        /// <param name="dir">생성할 디렉토리</param>
        /// <param name="id">사이트아이디</param>
        /// <param name="kno">사이트키</param>
        public void CreateDirectory(string dir)
        {
            // 웹서비스 자원 사용하기
            SVCFileUpLoad sfile = new SVCFileUpLoad();
            sfile.SetSaveDir(saveDir);
            sfile.CreateDirectory(dir);
        }

        #endregion

        #region 새 파일로 저장

        public List<string> NewFileUpload(System.Web.UI.WebControls.FileUpload files, string dir)
        {

            string ext = string.Empty;
            int fileLength = files.PostedFile.ContentLength;
            int fnStart = -1;
            int fnLen = 0;

            // 리턴변수
            List<string> Rtn = new List<string>();
            Rtn.Add(string.Empty); //[0] : 상태값
            Rtn.Add(string.Empty); //[1] : 파일명

            if (fileLength > maxLength)
            {
                RTN_STATUS = "NOSIZ";
            }
            else
            {

                filename = files.PostedFile.FileName;
                fnLen = filename.Length;
                fnStart = filename.LastIndexOf('\\') + 1;

                // 파일 유무 체크
                if (fnLen > 0)
                {
                    //if (fnStart > 0 && (fnLen - fnStart) > 0)
                    //{
                    Stream strm;

                    // 파일명만 추출하기
                    int extStart = -1;
                    int extLen = 0;
                    bool chkExt = false;

                    filename = filename.Substring(fnStart, fnLen - fnStart); // 파일명

                    // 파일명에서 확장자 추출하기
                    extLen = filename.Length;
                    extStart = filename.LastIndexOf('.') + 1;
                    if (extStart > 0 && (extLen - extStart) > 0)
                        ext = filename.Substring(extStart, extLen - extStart); // 확장자

                    // 확장자 체크
                    foreach (string s in listExt)
                        if (ext.ToUpper() == s)
                            chkExt = true;

                    string[] splitExt = filename.Split('.');

                    // 2017-05-19 yeeun, 확장자 체크 추가 - . 한개인지 체크함
                    if (splitExt.Length != 2)
                    {
                        chkExt = false;
                    }

                    if (chkExt)
                    {
                        byte[] send = new byte[fileLength];
                        strm = files.PostedFile.InputStream;
                        strm.Read(send, 0, fileLength);

                        // 웹서비스 자원 사용하기
                        SVCFileUpLoad sfile = new SVCFileUpLoad();
                        sfile.SetSaveDir(saveDir);
                        // 파일명만 넘겨주세요.
                        filename = sfile.NewFileSaveFile(send, dir, filename);
                        strm.Close();

                        if (!filename.Equals(string.Empty))
                            RTN_STATUS = "SUCES";
                        else
                            RTN_STATUS = "FAILD";
                    }
                    else
                        RTN_STATUS = "NOEXT";
                    //}
                    //else
                    //    RTN_STATUS = "NOFNM";
                }
                else
                    RTN_STATUS = "NOSEL";
            }

            Rtn[0] = RTN_STATUS;
            Rtn[1] = filename;
            return Rtn;
        }
        /// <summary>
        /// 새 파일 저장 - 같은 파일일 경우 새 이름으로 저장한다. 
        /// [0] 상태값
        /// [1] 파일명
        /// </summary>
        /// <param name="files">FileUpload컨트롤</param>
        /// <param name="dir">저장경로</param>
        /// <returns></returns>
        //public List<string> NewFileUpload(System.Web.UI.WebControls.FileUpload files, string dir)
        //{

        //    string ext = string.Empty;
        //    int fileLength = files.PostedFile.ContentLength;
        //    int fnStart = -1;
        //    int fnLen = 0;

        //    // 리턴변수
        //    List<string> Rtn = new List<string>();
        //    Rtn.Add(string.Empty); //[0] : 상태값
        //    Rtn.Add(string.Empty); //[1] : 파일명

        //    filename = files.PostedFile.FileName;
        //    fnLen = filename.Length;
        //    fnStart = filename.LastIndexOf('\\') + 1;

        //    // 파일 유무 체크
        //    if (fnLen > 0)
        //    {
        //        //if (fnStart > 0 && (fnLen - fnStart) > 0)
        //        //{
        //        Stream strm;

        //        // 파일명만 추출하기
        //        int extStart = -1;
        //        int extLen = 0;
        //        bool chkExt = false;

        //        filename = filename.Substring(fnStart, fnLen - fnStart); // 파일명

        //        // 파일명에서 확장자 추출하기
        //        extLen = filename.Length;
        //        extStart = filename.LastIndexOf('.') + 1;
        //        if (extStart > 0 && (extLen - extStart) > 0)
        //            ext = filename.Substring(extStart, extLen - extStart); // 확장자

        //        // 확장자 체크
        //        foreach (string s in listExt)
        //            if (ext.ToUpper() == s)
        //                chkExt = true;

        //        string[] splitExt = filename.Split('.');

        //        // 2017-05-19 yeeun, 확장자 체크 추가 - . 한개인지 체크함
        //        if (splitExt.Length != 2)
        //        {
        //            chkExt = false;
        //        }

        //        if (chkExt)
        //        {
        //            byte[] send = new byte[fileLength];
        //            strm = files.PostedFile.InputStream;
        //            strm.Read(send, 0, fileLength);

        //            // 웹서비스 자원 사용하기
        //            SVCFileUpLoad sfile = new SVCFileUpLoad();
        //            sfile.SetSaveDir(saveDir);
        //            // 파일명만 넘겨주세요.
        //            filename = sfile.NewFileSaveFile(send, dir, filename);
        //            strm.Close();

        //            if (!filename.Equals(string.Empty))
        //                RTN_STATUS = "SUCES";
        //            else
        //                RTN_STATUS = "FAILD";
        //        }
        //        else
        //            RTN_STATUS = "NOEXT";
        //        //}
        //        //else
        //        //    RTN_STATUS = "NOFNM";
        //    }
        //    else
        //        RTN_STATUS = "NOSEL";

        //    Rtn[0] = RTN_STATUS;
        //    Rtn[1] = filename;
        //    return Rtn;
        //}

        #endregion

        #region 새 파일로 저장 - HttpFileCollection
        /// <summary>
        /// 새 파일 저장 - 같은 파일일 경우 새 이름으로 저장한다. 
        /// [0] 상태값
        /// [1] 파일명
        /// </summary>
        /// <param name="files">FileUpload컨트롤</param>
        /// <param name="dir">저장경로</param>
        /// <returns></returns>
        public List<string> NewFileUpload_Collection(HttpPostedFile files, string dir, string key)
        {
            string ext = string.Empty;
            int fileLength = files.ContentLength;
            int fnStart = -1;
            int fnLen = 0;
            int fileSize = 0;

            // 리턴변수
            List<string> Rtn = new List<string>();
            //Rtn.Add(string.Empty); //[0] : 상태값
            //Rtn.Add(string.Empty); //[1] : 파일명

            if (fileLength > maxLength)
            {
                RTN_STATUS = "NOSIZ";
            }
            else
            {

                filename = files.FileName;
                fnLen = filename.Length;
                fnStart = filename.LastIndexOf('\\') + 1;

                // 파일 유무 체크
                if (fnLen > 0)
                {
                    //if (fnStart > 0 && (fnLen - fnStart) > 0)
                    //{
                    Stream strm;

                    // 파일명만 추출하기
                    int extStart = -1;
                    int extLen = 0;
                    bool chkExt = false;

                    filename = filename.Substring(fnStart, fnLen - fnStart); // 파일명

                    // 파일명에서 확장자 추출하기
                    extLen = filename.Length;
                    extStart = filename.LastIndexOf('.') + 1;
                    if (extStart > 0 && (extLen - extStart) > 0)
                        ext = filename.Substring(extStart, extLen - extStart); // 확장자

                    // 확장자 체크
                    foreach (string s in listExt)
                        if (ext.ToUpper() == s)
                            chkExt = true;


                    if (chkExt)
                    {
                        byte[] send = new byte[fileLength];
                        strm = files.InputStream;
                        fileSize = strm.Read(send, 0, fileLength);

                        if (fileSize <= maxLength)
                        {

                            // 웹서비스 자원 사용하기
                            SVCFileUpLoad sfile = new SVCFileUpLoad();
                            sfile.SetSaveDir(saveDir);
                            // 파일명만 넘겨주세요.
                            filename = sfile.NewFileSaveFile(send, dir, filename);
                            strm.Close();

                            if (!filename.Equals(string.Empty))
                                RTN_STATUS = "SUCES";
                            else
                                RTN_STATUS = "FAILD";
                        }
                        else
                        {
                            RTN_STATUS = "NOSIZ";
                        }
                    }
                    else
                        RTN_STATUS = "NOEXT";
                    //}
                    //else
                    //    RTN_STATUS = "NOFNM";
                }
                else
                    RTN_STATUS = "NOSEL";
            }

            Rtn.Add(RTN_STATUS);
            Rtn.Add(key);
            Rtn.Add(filename);
            Rtn.Add(dir);
            Rtn.Add(fileSize.ToString());
            Rtn.Add(ext);


            return Rtn;
        }

        /// <summary>
        /// 새 파일 저장 - 같은 파일일 경우 새 이름으로 저장한다. (파일명 변경 저장)
        /// [0] 상태값
        /// [1] 파일명
        /// </summary>
        /// <param name="files">FileUpload컨트롤</param>
        /// <param name="dir">저장경로</param>
        /// <returns></returns>
        public List<string> NewFileUpload_Collection(HttpPostedFile files, string dir, string key, string fileName)
        {
            string ext = string.Empty;
            int fileLength = files.ContentLength;
            int fnStart = -1;
            int fnLen = 0;
            int fileSize = 0;

            // 리턴변수
            List<string> Rtn = new List<string>();
            //Rtn.Add(string.Empty); //[0] : 상태값
            //Rtn.Add(string.Empty); //[1] : 파일명

            if (fileLength > maxLength)
            {
                RTN_STATUS = "NOSIZ";
            }
            else
            {

                filename = fileName;
                fnLen = filename.Length;
                fnStart = filename.LastIndexOf('\\') + 1;

                // 파일 유무 체크
                if (fnLen > 0)
                {
                    //if (fnStart > 0 && (fnLen - fnStart) > 0)
                    //{
                    Stream strm;

                    // 파일명만 추출하기
                    int extStart = -1;
                    int extLen = 0;
                    bool chkExt = false;

                    filename = filename.Substring(fnStart, fnLen - fnStart); // 파일명

                    // 파일명에서 확장자 추출하기
                    extLen = filename.Length;
                    extStart = filename.LastIndexOf('.') + 1;
                    if (extStart > 0 && (extLen - extStart) > 0)
                        ext = filename.Substring(extStart, extLen - extStart); // 확장자

                    // 확장자 체크
                    foreach (string s in listExt)
                        if (ext.ToUpper() == s)
                            chkExt = true;


                    if (chkExt)
                    {
                        byte[] send = new byte[fileLength];
                        strm = files.InputStream;
                        fileSize = strm.Read(send, 0, fileLength);

                        if (fileSize <= maxLength)
                        {

                            // 웹서비스 자원 사용하기
                            SVCFileUpLoad sfile = new SVCFileUpLoad();
                            sfile.SetSaveDir(saveDir);
                            // 파일명만 넘겨주세요.
                            filename = sfile.NewFileSaveFile(send, dir, filename);
                            strm.Close();

                            if (!filename.Equals(string.Empty))
                                RTN_STATUS = "SUCES";
                            else
                                RTN_STATUS = "FAILD";
                        }
                        else
                        {
                            RTN_STATUS = "NOSIZ";
                        }
                    }
                    else
                        RTN_STATUS = "NOEXT";
                    //}
                    //else
                    //    RTN_STATUS = "NOFNM";
                }
                else
                    RTN_STATUS = "NOSEL";
            }

            Rtn.Add(RTN_STATUS);
            Rtn.Add(key);
            Rtn.Add(filename);
            Rtn.Add(dir);
            Rtn.Add(fileSize.ToString());
            Rtn.Add(ext);


            return Rtn;
        }

        #endregion

        #region 기존파일명으로 파일 저장
        /// <summary>
        /// 기존파일명으로 파일 저장 : 같은 이름일 경우 덮어씌운다.
        /// [0] 상태값
        /// [1] 파일명
        /// </summary>
        /// <param name="files">FileUpload컨트롤</param>
        /// <param name="dir">저장경로</param>
        /// <returns></returns>
        public List<string> SameFileUpload(System.Web.UI.WebControls.FileUpload files, string dir)
        {

            string ext = string.Empty;
            int fileLength = files.PostedFile.ContentLength;
            int fnStart = -1;
            int fnLen = 0;

            // 리턴변수
            List<string> Rtn = new List<string>();
            Rtn.Add(string.Empty); //[0] : 상태값
            Rtn.Add(string.Empty); //[1] : 파일명
            filename = files.PostedFile.FileName;
            fnLen = filename.Length;
            fnStart = filename.LastIndexOf('\\') + 1;

            // 파일 유무 체크
            if (fnLen > 0)
            {
                if (fnStart > 0 && (fnLen - fnStart) > 0)
                {
                    // 파일명만 추출하기
                    int extStart = -1;
                    int extLen = 0;
                    bool chkExt = false;
                    filename = filename.Substring(fnStart, fnLen - fnStart); // 파일명

                    // 파일명에서 확장자 추출하기
                    extLen = filename.Length;
                    extStart = filename.LastIndexOf('.') + 1;
                    if (extStart > 0 && (extLen - extStart) > 0)
                        ext = filename.Substring(extStart, extLen - extStart); // 확장자

                    // 확장자 체크
                    foreach (string s in listExt)
                        if (ext.ToUpper() == s)
                            chkExt = true;

                    if (chkExt)
                    {
                        Stream strm;
                        byte[] send = new byte[fileLength];
                        strm = files.PostedFile.InputStream;
                        strm.Read(send, 0, fileLength);

                        // 웹서비스 자원 사용하기
                        SVCFileUpLoad sfile = new SVCFileUpLoad();
                        sfile.SetSaveDir(saveDir);

                        // 파일명만 넘겨주세요.
                        filename = sfile.SameFileSaveFile(send, dir, filename);
                        strm.Close();

                        if (!filename.Equals(string.Empty))
                            RTN_STATUS = "SUCES";
                        else
                            RTN_STATUS = "FAILD";
                    }
                    else
                        RTN_STATUS = "NOEXT";
                }
                else
                    RTN_STATUS = "NOFNM";
            }
            else
                RTN_STATUS = "NOSEL";

            Rtn[0] = RTN_STATUS;
            Rtn[1] = filename;

            return Rtn;
        }

        #endregion

        #region 다중 새 파일로 저장
        /// <summary>
        /// 다중 새 파일 저장 - 같은 파일일 경우 새 이름으로 저장한다. 
        /// [2][0] 전체 상태값
        /// </summary>
        /// <param name="files"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public List<List<string>> MultiNewFileUpload(List<System.Web.UI.WebControls.FileUpload> files, string dir)
        {
            string CHK_STATUS = "FAILD";

            // 리턴변수
            List<List<string>> Rtn = new List<List<string>>();
            List<string> RtnFile = new List<string>();
            List<string> RtnStatus = new List<string>();
            List<string> RtnFullStatus = new List<string>();
            RtnFullStatus.Add(string.Empty);
            List<string> svcRtn = new List<string>();

            foreach (System.Web.UI.WebControls.FileUpload f in files)
            {
                svcRtn = NewFileUpload(f, dir);
                RtnStatus.Add(svcRtn[0]); // 상태값
                RtnFile.Add(svcRtn[1]); // 파일명
            }

            foreach (string s in RtnStatus)
            {
                if (s == "SUCES" || s == "NOSEL")
                    CHK_STATUS = "SUCES";
            }
            RtnFullStatus[0] = CHK_STATUS;
            Rtn.Add(RtnStatus); // 각 업로드 상태
            Rtn.Add(RtnFile); // 각 업로드 파일명
            Rtn.Add(RtnFullStatus); // 전체 상태
            return Rtn;
        }

        #endregion

        #region 다중 기존 파일로 저장
        /// <summary>
        /// 다중 기존 파일로 저장 - 같은 이름일 경우 덮어씌운다.
        /// [2][0] 전체 상태값
        /// </summary>
        /// <param name="files"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public List<List<string>> MultiSameFileUpload(List<System.Web.UI.WebControls.FileUpload> files, string dir)
        {
            string CHK_STATUS = "FAILD";
            // 리턴변수
            List<List<string>> Rtn = new List<List<string>>();
            List<string> RtnFile = new List<string>();
            List<string> RtnStatus = new List<string>();
            List<string> RtnFullStatus = new List<string>();
            RtnFullStatus.Add(string.Empty);
            List<string> svcRtn = new List<string>();

            foreach (System.Web.UI.WebControls.FileUpload f in files)
            {
                svcRtn = SameFileUpload(f, dir);
                RtnStatus.Add(svcRtn[0]); // 상태값
                RtnFile.Add(svcRtn[1]); // 파일명
            }

            foreach (string s in RtnStatus)
            {
                if (s == "SUCES" || s == "NOSEL")
                    CHK_STATUS = "SUCES";
            }
            RtnFullStatus[0] = CHK_STATUS;
            Rtn.Add(RtnStatus); // 각 업로드 상태
            Rtn.Add(RtnFile); // 각 업로드 파일명
            Rtn.Add(RtnFullStatus); // 전체 상태
            return Rtn;

        }

        #endregion

        #region 다중 새 파일로 저장 - HttpFileCollection
        /// <summary>
        /// 다중 새 파일 저장 - 같은 파일일 경우 새 이름으로 저장한다. 
        /// [2][0] 전체 상태값
        /// </summary>
        /// <param name="files"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public List<List<string>> MultiNewFileUpload_Collection(HttpFileCollection files, string dir)
        {
            string CHK_STATUS = "FAILD";

            // 리턴변수
            List<List<string>> Rtn = new List<List<string>>();
            List<string> RtnFile = new List<string>();
            List<string> RtnStatus = new List<string>();
            List<string> RtnFullStatus = new List<string>();
            RtnFullStatus.Add(string.Empty);
            List<string> svcRtn = new List<string>();

            for (int i = 0; i < files.Count; i++)
            {
                if (files[i].FileName != "")
                {
                    svcRtn = NewFileUpload_Collection(files[i], dir, files.Keys[i]);
                    Rtn.Add(svcRtn);
                }
            }

            foreach (string s in RtnStatus)
            {
                if (s == "SUCES" || s == "NOSEL")
                    CHK_STATUS = "SUCES";
            }

            return Rtn;
        }

        #endregion

        #region 파일 삭제
        /// <summary>
        /// 파일 삭제
        /// </summary>
        /// <param name="dir">디렉토리</param>
        /// <param name="filename">파일명</param>
        public string DeleteFile(string dir, string _filename)
        {
            bool delCheck = false;
            string RTN_STATUS = string.Empty;
            // 웹서비스 자원 사용하기
            SVCFileUpLoad sfile = new SVCFileUpLoad();
            sfile.SetSaveDir(saveDir);

            delCheck = sfile.DeleteFile(dir, _filename);

            if (delCheck)
                RTN_STATUS = "SUCES";
            else
                RTN_STATUS = "FAILD";

            return RTN_STATUS;
        }

        #endregion

        #region 상태 메세지
        /// <summary>
        /// 상태 메세지
        /// </summary>
        /// <param name="status">상태코드 5자리</param>
        /// <returns></returns>
        public string SatausMessage(string status)
        {
            /*
            상태 변수
            READY : 준비
            NOEXT : 지원되지 않는 확장자
            NOFNM : 잘못된 파일명
            FAILD : 서버에서 실패
            SUCES : 성공
            NOSEL : 파일선택안함
            */
            string rtnMessage = string.Empty;

            switch (status)
            {
                case "READY":
                    rtnMessage = "이미지 업로드 대기중입니다.";
                    break;
                case "NOEXT":
                    rtnMessage = "이미지확장자가 지원되지 않는 확장자입니다.";
                    break;
                case "NOFNM":
                    rtnMessage = "파일명이 잘못되었습니다.";
                    break;
                case "FAILD":
                    rtnMessage = "이미지 업로드가 실패했습니다.";
                    break;
                case "SUCES":
                    rtnMessage = "이미지가 성공적으로 업로드 되었습니다.";
                    break;
                case "NOSEL":
                    rtnMessage = "이미지 파일을 선택하지 않았습니다.";
                    break;
                case "NOSIZ":
                    rtnMessage = "이미지 용량이 초과되었습니다.";
                    break;
            }
            return rtnMessage;
        }
        #endregion
    }
}