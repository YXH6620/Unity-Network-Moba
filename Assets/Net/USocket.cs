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

        string ip = "10.254.5.81"; // ����������
        int port = 8899; // ����������

        public static IPEndPoint server;

        public USocket(Action<BufferEntity> dispatchNetEvent)
        {
            udpClient = new UdpClient(0);
            server = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        /// <summary>
        /// ���ܱ���
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
        /// ���ͱ��Ľӿ�
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
                    Debug.LogError($"�����쳣:{e.Message}");
                }
            }
        }

        /// <summary>
        /// ����ACK���� ��������ϵ���
        /// </summary>
        /// <param name="bufferEntity"></param>
        public void SendACK(BufferEntity bufferEntity)
        {
            Send(bufferEntity.buffer, server);
        }

        /// <summary>
        /// �ر�udpClient
        /// </summary>
        public void Close()
        {
            udpClient.Close();
            udpClient = null;
        }
    }
}


