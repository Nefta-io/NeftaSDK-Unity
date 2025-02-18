using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Nefta;
using UnityEngine;

namespace AdDemo
{
    public class DebugServer
    {
        private TcpClient _client;
        private Thread _backgroundThread;
        private NetworkStream _stream;
        private Queue<string> _messageQueue;
        
        private string _ip;
        
        public void Init(string ip)
        {
            _ip = ip;

            _messageQueue = new Queue<string>();
            
            _backgroundThread = new Thread(new ThreadStart(ListenForIncomingRequests));
            _backgroundThread.IsBackground = true;
            _backgroundThread.Start();
        }
        
        private void ListenForIncomingRequests()
        {
            try
            {
                _client = new TcpClient();
                _client.Connect(_ip, 12013);
                _stream = _client.GetStream();
                
                Debug.Log("DS:Connection established");
                Send("log", "Debug server connected");
                
                byte[] buffer = new byte[1024];
                int length;
                while ((length = _stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    var data = new byte[length];
                    Array.Copy(buffer, 0, data, 0, length);
                    string message = Encoding.UTF8.GetString(data);
                    lock (_messageQueue)
                    {
                        _messageQueue.Enqueue(message);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.Log($"Exception {exception}");
            }
        }

        public void OnUpdate()
        {
            lock (_messageQueue)
            {
                while (_messageQueue.Count > 0)
                {
                    string message = _messageQueue.Dequeue();
                     Debug.Log("Processing: " + message);

                    var control = message;
                    var controlEnd = message.IndexOf(" ");
                    if (controlEnd != -1)
                    {
                        control = message.Substring(0, controlEnd);
                    }
                    if (control == "get_nuid")
                    {
                        var nuid = NeftaAds.Instance.GetNuid(true);
                        Send("return nuid", nuid);
                    }
                    else if (control == "ad_units")
                    {
                        string adUnits = "{\"ad_units\":[";
                        var first = true;
                        foreach (var placement in NeftaAds.Instance.Placements)
                        {
                            if (first)
                            {
                                first = false;
                            }
                            else
                            {
                                adUnits += ",";
                            }
                            adUnits += "{\"id\":\"" + placement.Key + "\",\"type\":\"";
                            if (placement.Value._type == AdUnit.Type.Banner)
                            {
                                adUnits += "banner\"}";
                            }
                            else if (placement.Value._type == AdUnit.Type.Interstitial)
                            {
                                adUnits += "interstitial\"}";
                            }
                            else
                            {
                                adUnits += "rewarded_video\"}";
                            }
                        }
                        Send("return ad_units", adUnits + "]}");
                    }
                    else if (control == "partial_bid")
                    {
                        var pId = message.Substring(controlEnd + 1);
                        var partialBid = NeftaAds.Instance.GetPartialBidRequest(pId);
                        Send("return partial_bid", partialBid);
                    }
                    else if (control == "bid")
                    {
                        var pId = message.Substring(controlEnd + 1);
                        NeftaAds.Instance.Bid(pId);
                        Send("return", "bid");
                    }
                    else if (control == "custom_load")
                    {
                        var pIdEnd = message.IndexOf(" ", controlEnd + 1, StringComparison.InvariantCulture);
                        var pId = message.Substring(controlEnd + 1, pIdEnd - controlEnd - 1);
                        var bidResponse = message.Substring(pIdEnd + 1);
                        NeftaAds.Instance.LoadWithBidResponse(pId, bidResponse);
                        Send("return", "custom_load");
                    }
                    else if (control == "load")
                    {
                        var pId = message.Substring(controlEnd + 1);
                        NeftaAds.Instance.Load(pId);
                        Send("return", "load");
                    }
                    else if (control == "show")
                    {
                        var pId = message.Substring(controlEnd + 1);
                        NeftaAds.Instance.Show(pId);
                        Send("return", "show");
                    }
                    else
                    {
                        Debug.Log($"Unknown control: {control}");
                    }
                }
            }
        }

        public void Send(string type, string message)
        {
            Send($"Uni {type} {message}");
        }

        private void Send(string data)
        {
            byte[] serverMessage = Encoding.UTF8.GetBytes(data);
            _stream.Write(serverMessage, 0, serverMessage.Length);
        }
    }
}