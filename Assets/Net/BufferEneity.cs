using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.VersionControl;
using UnityEngine;


// ����ͨ�ű��ĵĸ�ʽ
namespace Game.Net
{
    public class BufferEntity
    {
        public int recurCount = 0; // �ط����� �����ڲ��õ� ����ҵ������
        public IPEndPoint endPoint; // ���͵�Ŀ���ն�

        public int protoSize;
        public int session; // �ỰID
        public int sn; // ���
        public int moduleID; // ģ��ID
        public long time; // ����ʱ��
        public int messageType; // Э������
        public int messageID; // Э��ID
        public byte[] proto; // ҵ����

        public byte[] buffer; // ����Ҫ���͵����� �� �յ�������

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="session"></param>
        /// <param name="sn"></param>
        /// <param name="moduleID"></param>
        /// <param name="messageType"></param>
        /// <param name="messageID"></param>
        /// <param name="proto"></param>
        public BufferEntity(IPEndPoint endPoint, int session, int sn, int moduleID, int messageType, int messageID, byte[] proto)
        {
            protoSize = proto.Length;
            this.endPoint = endPoint;
            this.session = session;
            this.sn = sn;
            this.moduleID = moduleID;
            this.messageType = messageType;
            this.messageID = messageID;
            this.proto = proto; 
        }

        /// <summary>
        /// �������յ��ı���ʵ��
        /// </summary>
        /// <param name="endPoint">�ն�IP�Ͷ˿�</param>
        /// <param name="buffer">�յ�������</param>
        public BufferEntity(IPEndPoint endPoint, byte[] buffer)
        {
            this.endPoint = endPoint;
            this.buffer = buffer;

            Decode();
        }

        /// <summary>
        /// ACK����ʵ��
        /// </summary>
        /// <param name="package"></param>
        public BufferEntity(BufferEntity package)
        {
            protoSize = 0;
            this.endPoint = package.endPoint; 
            this.session= package.session; 
            this.sn = package.sn;
            this.moduleID = package.moduleID;
            this.time = package.time;
            this.messageType = package.messageType;
            this.messageID = package.messageID;

            buffer = Encoder(true);
        }

        // ����ӿ� Encoder, ACKȷ�ϱ��ģ�ҵ����
        public byte[] Encoder(bool isAck)
        {
            byte[] data = new byte[32 + protoSize];

            byte[] _length = BitConverter.GetBytes(protoSize);
            byte[] _session = BitConverter.GetBytes(session);
            byte[] _sn = BitConverter.GetBytes(sn);
            byte[] _moduleid = BitConverter.GetBytes(moduleID);
            byte[] _time = BitConverter.GetBytes(time);
            byte[] _messageType = BitConverter.GetBytes(messageType);
            byte[] _messageID = BitConverter.GetBytes(messageID);

            Array.Copy(_length, 0, data, 0, 4);
            Array.Copy(_session, 0, data, 4, 4);
            Array.Copy(_sn, 0, data, 8, 4);
            Array.Copy(_moduleid, 0, data, 12, 4);
            Array.Copy(_time, 0, data, 16, 8);
            Array.Copy(_messageType, 0, data, 24, 4);
            Array.Copy(_messageID, 0, data, 28, 4);

            if(isAck)
            {

            }
            else
            {
                // ��ҵ������ ׷�ӽ���
                Array.Copy(proto, 0, data, 32, proto.Length);
            }

            buffer = data;
            return data;
        }

        // �����ķ����л� Ϊ��Ա
        private void Decode()
        {
            protoSize = BitConverter.ToInt32(buffer, 0); // ��0��λ�ã�ȡ4���ֽ�ת����int
            session = BitConverter.ToInt32(buffer, 4);
            sn = BitConverter.ToInt32(buffer, 8);
            moduleID = BitConverter.ToInt32(buffer, 12);

            time = BitConverter.ToInt64(buffer, 16);

            messageType = BitConverter.ToInt32(buffer, 24);
            messageID = BitConverter.ToInt32(buffer, 28);

            // 
            if(messageType == 0)
            {

            }
            else
            {
                proto = new byte[protoSize];
                // ��buffer��ʣ�µ����� ���Ƶ�protp �õ����յ�ҵ������
                Array.Copy(buffer, 32, proto, 0, protoSize);
            }
        }
    }
}

public class BufferEneity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
