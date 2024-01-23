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
