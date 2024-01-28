using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace Game.Net
{
    public class USocket
    {
        UdpClient udpClient;

        string ip = "10.254.5.81"; // 服务器主机
        int port = 8899; // 服务器程序

        public static IPEndPoint server;

        public USocket(Action<BufferEntity> dispatchNetEvent)
        {
            udpClient = new UdpClient(0);
            server = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        /// <summary>
        /// 接受报文
        /// </summary>
        ConcurrentQueue<UdpReceiveResult> awaitHandle = new ConcurrentQueue<UdpReceiveResult>();
        public async void ReceiveTask()
        {
            while(udpClient!=null)
            {
                try
                {
                    UdpReceiveResult result = await udpClient.ReceiveAsync();
                    awaitHandle.Enqueue(result);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        /// <summary>
        /// 发送报文接口
        /// </summary>
        /// <param name="data"></param>
        /// <param name="endPoint"></param>
        public async void Send(byte[] data,IPEndPoint endPoint)
        {
            if(udpClient!=null)
            {
                try
                {
                    int length = await udpClient.SendAsync(data, data.Length, ip, port);
                }
                catch(Exception e)
                {
                    Debug.LogError($"发送异常:{e.Message}");
                }
            }
        }

        /// <summary>
        /// 发送ACK报文 解包后马上调用
        /// </summary>
        /// <param name="bufferEntity"></param>
        public void SendACK(BufferEntity bufferEntity)
        {
            Send(bufferEntity.buffer, server);
        }

        /// <summary>
        /// 关闭udpClient
        /// </summary>
        public void Close()
        {
            udpClient.Close();
            udpClient = null;
        }
    }
}


