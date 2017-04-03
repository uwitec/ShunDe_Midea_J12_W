using System;
using System.Collections.Generic;
using System.Text;

namespace NewMideaProgram
{
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public class ErrDataInfo
    {
        /// <summary>
        /// �Ƿ���ʾ
        /// </summary>
        public bool isShow;
        /// <summary>
        /// ������Ϣ
        /// </summary>
        public string data;
        /// <summary>
        /// ������Ϣ����
        /// </summary>
        public string errDataEnum;
        public void SetErrorStatue(bool statue)
        {
            isShow = statue;
        }
    }
    public enum ErrDataEnum
    {
        Info,
        Error,
        Protect
    }
    public class cError
    {
        /// <summary>
        /// ������Ϣ��
        /// </summary>
        List<ErrDataInfo> flushErrDataInfo = new List<ErrDataInfo>();//��Ҫ�����ǰѴ�����Ϣ�ֿ���ʾ,������ɲ�ͬ����Ϣ��ʾ����ˢ�µ�����
        /// <summary>
        /// ��ǰ������Ϣ��
        /// </summary>
        int  NowShowErrIndex =0;
        object o = new object();
        /// <summary>
        /// ��Ӵ�����Ϣ
        /// </summary>
        /// <param name="name">string,������Ϣ��,���ڸ�λʱ</param>
        /// <param name="ErrorDescribe">string,������Ϣ������</param>
        public void AddErrData(string name,string ErrorDescribe)
        {
            for (int i = 0; i < flushErrDataInfo.Count; i++)
            {
                if (flushErrDataInfo[i].errDataEnum == name)//�԰����������
                {
                    flushErrDataInfo[i].data = ErrorDescribe;
                    return;
                }
            }
            ErrDataInfo errDataInfo = new ErrDataInfo();
            errDataInfo.isShow = true;
            errDataInfo.errDataEnum = name;
            errDataInfo.data = ErrorDescribe;
            lock (o)
            {
                flushErrDataInfo.Add(errDataInfo);
            }
        }
        public void DelErrData(string name)
        {
            for (int i = 0; i < flushErrDataInfo.Count; i++)
            {
                if (flushErrDataInfo[i].errDataEnum == name)
                {
                    lock (o)
                    {
                        flushErrDataInfo.Remove(flushErrDataInfo[i]);
                    }
                }
            }
        }
        public string GetError()
        {
            string s = "";
            lock (o)
            {
                NowShowErrIndex++;
                if (flushErrDataInfo != null)
                {
                    if (flushErrDataInfo.Count > 0)
                    {
                        NowShowErrIndex = NowShowErrIndex % flushErrDataInfo.Count;
                        s = flushErrDataInfo[NowShowErrIndex].data;
                    }
                }
                
            }
            return s;
        }
        public ErrDataEnum GetErrEnum()
        {
            ErrDataEnum errDataEnum = ErrDataEnum.Info;
            if (flushErrDataInfo != null)
            {
                if (flushErrDataInfo.Count > 0)
                {
                    if (flushErrDataInfo[NowShowErrIndex].errDataEnum.IndexOf("ERROR") >= 0)
                    {
                        errDataEnum = ErrDataEnum.Error;
                    }
                    if (flushErrDataInfo[NowShowErrIndex].errDataEnum.IndexOf("PROTECT") >= 0)
                    {
                        errDataEnum = ErrDataEnum.Protect;
                    }
                }
            }
            return errDataEnum;
        }
    }
}
