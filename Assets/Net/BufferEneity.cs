using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.VersionControl;
using UnityEngine;


// 定制通信报文的格式
namespace Game.Net
{
    public class BufferEntity
    {
        public int recurCount = 0; // 重发次数 工程内部用到 并非业务数据
        public IPEndPoint endPoint; // 发送的目标终端

        public int protoSize;
        public int session; // 会话ID
        public int sn; // 序号
        public int moduleID; // 模块ID
        public long time; // 发送时间
        public int messageType; // 协议类型
        public int messageID; // 协议ID
        public byte[] proto; // 业务报文

        public byte[] buffer; // 最终要发送的数据 或 收到的数据

        /// <summary>
        /// 构建请求报文
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
        /// 构建接收到的报文实体
        /// </summary>
        /// <param name="endPoint">终端IP和端口</param>
        /// <param name="buffer">收到的数据</param>
        public BufferEntity(IPEndPoint endPoint, byte[] buffer)
        {
            this.endPoint = endPoint;
            this.buffer = buffer;

            Decode();
        }

        /// <summary>
        /// ACK报文实体
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

        // 编码接口 Encoder, ACK确认报文，业务报文
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
                // 是业务数据 追加进来
                Array.Copy(proto, 0, data, 32, proto.Length);
            }

            buffer = data;
            return data;
        }

        // 将报文反序列化 为成员
        private void Decode()
        {
            protoSize = BitConverter.ToInt32(buffer, 0); // 从0的位置，取4个字节转化成int
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
                // 将buffer里剩下的数据 复制到protp 得到最终的业务数据
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
