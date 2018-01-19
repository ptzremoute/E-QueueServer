﻿using CoreNetLib;
using EQueueLib;
using Microsoft.Extensions.Logging;

namespace QueueServerLib
{
    public class QueueServer
    {
        internal IDb db;
        internal IHub hub;
        QueueServerState qsState;

        ILogger Logger { get; } =
            CoreNetLogging.LoggerFactory.CreateLogger<QueueServer>();
        
        public QueueServer()
            : this(new TcpHub(), new QueueDB())
        {

        }

        public QueueServer(IHub hub, IDb db)
        {
            QueueDB.Init();
            this.hub = hub;
            this.db = db;
            qsState = new QueueServerState();

            hub.OnDataReceived += TcpServer_OnDataReceived;
        }

        public void Run()
        {
            hub.Start();
        }

        public void QueueInitialize()
        {
            foreach (var clientInfo in db.GetClientInQueue())
            {
                qsState.ClientInQueue.AddOrUpdateNewClient(clientInfo);
            }

            var lastClient = db.GetLastClient();

            if (lastClient != null)
            {
                qsState.CurentClientNumber = lastClient.ClientNumber;
            }
        }

        private void TcpServer_OnDataReceived(object sender, ReceivedDataEventArgs e)
        {
            var receivedQueueData = (QueueData)e.Data;

            //TODO: change to interlock
            lock (this)
            {
                Logger.LogTrace($"Command {receivedQueueData.Cmd} received.");
                var handler = DataHandlerFactory.MakeHandler(receivedQueueData);
                var handledData = handler.Handle(qsState, db);

                db.SaveChanges();

                if (qsState.TryGetNextClient(out QueueClientInfo nextClient))
                {
                    var nextClientData = QueueDataFactory.GetNextClientData(nextClient);
                    db.SaveChanges();
                    SendDataToAll(nextClientData);
                }

                if (handledData != null)
                {
                    SendDataToAll(handledData);
                }
            }
        }

        internal QueueState GetQueueState()
        {
            return new QueueState
            {
                ServiceList = db.GetServiceList(),
                OperatorList = db.GetOperatorList(),
                ClientInQueue = db.GetClientInQueue()
            };
        }

        void SendDataToAll(QueueData qData)
        {
            if (qData != null)
            {
                qData.QueueState = GetQueueState();
                hub.SendToAll(qData);
            }
        }
    }
}
