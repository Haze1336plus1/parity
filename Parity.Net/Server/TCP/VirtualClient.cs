using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Parity.Net.Server.TCP
{
    public class VirtualClient
    {

        public System.Net.Sockets.Socket Socket { get; private set; }
        public System.Net.IPEndPoint RemoteEndPoint { get; private set; }
        public bool IsDisconnected { get; private set; }
        public string DisconnectReason { get; private set; }
        private System.Net.Sockets.SocketError _ReceiveError;
        private byte[] _ReceiveBuffer;

        public VirtualClient(byte[] _receiveBuffer, System.Net.Sockets.Socket _Socket)
        {
            this.Socket = _Socket;
            this.RemoteEndPoint = (System.Net.IPEndPoint)_Socket.RemoteEndPoint;
            this._ReceiveBuffer = _receiveBuffer;
            this.Socket.NoDelay = true;
        }

        public void Send(byte[] data)
        {
            try
            {
                if (this.Socket != null)
                {
                    if (this.Socket.Connected)
                        this.Socket.Send(data, System.Net.Sockets.SocketFlags.None);
                    else
                        this.Disconnect();
                }
            }
            catch
            {
                if ((this.Socket != null) && !this.Socket.Connected)
                    this.Disconnect();
            }
        }
        public void Disconnect()
        {
            try
            {
                this.Socket.Disconnect(true);
            }
            catch { }
            finally
            {
                this.Socket = null;
            }
            bool raiseEvent = !this.IsDisconnected;
            this.IsDisconnected = true;
            if (raiseEvent && OnDisconnected != null)
                OnDisconnected(this, this._ReceiveError);
        }
        public void Disconnect(string reason)
        {
            if (this.IsDisconnected)
                return;
            this.DisconnectReason = reason;
            this.Disconnect();
        }
 
        internal void BeginListen()
        {
            if (this.Socket.Connected)
            {
                try
                {
                    this.Socket.BeginReceive(this._ReceiveBuffer, 0, this._ReceiveBuffer.Length, System.Net.Sockets.SocketFlags.None, out this._ReceiveError, OnReceive, this);
                }
                catch
                {
                    this.Disconnect();
                }
            }
            else
            {
                this.Disconnect();
            }
        }
        internal static void OnReceive(IAsyncResult ar)
        {
            VirtualClient vc = ar.AsyncState as VirtualClient;
            if (vc != null && vc.Socket != null && vc.Socket.Connected)
            {
                int Received = 0;
                try
                {
                    Received = vc.Socket.EndReceive(ar);
                }
                catch
                {
                    if (vc.Socket.Connected)
                        vc.Disconnect();
                    return;
                }
                if (Received > 0)
                {
                    byte[] myBytes = new byte[Received];
                    Array.Copy(vc._ReceiveBuffer, myBytes, Received);
                    Array.Clear(vc._ReceiveBuffer, 0, vc._ReceiveBuffer.Length);
                    if (vc.Socket.Connected)
                    {
                        if (vc.OnDataReceived != null)
                            vc.OnDataReceived(vc, myBytes);
                        if (vc.Socket != null && vc.Socket.Connected)
                        {
                            vc.Socket.BeginReceive(vc._ReceiveBuffer, 0, vc._ReceiveBuffer.Length, System.Net.Sockets.SocketFlags.None, out vc._ReceiveError, OnReceive, vc);
                            return;
                        }
                    }
                }
            }
            vc.Disconnect();
        }

        public event OnDisconnectedEventHandler OnDisconnected;
        public delegate void OnDisconnectedEventHandler(VirtualClient sender, System.Net.Sockets.SocketError ErrorCode);
        public event OnDataReceivedEventHandler OnDataReceived;
        public delegate void OnDataReceivedEventHandler(VirtualClient sender, byte[] data);
    }
}