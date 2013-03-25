﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SomDB.Driver
{
	public class SomDBTransaction : IDisposable
	{		
		private bool m_transactionCompletionHandled = false;

		public SomDBTransaction(SomDBConnection connection, int transactionId)
		{
			Connection = connection;
			
			TransactionIdBytes = BitConverter.GetBytes(transactionId);
		}

		internal byte[] TransactionIdBytes { get; private set; }
		public SomDBConnection Connection { get; private set; }
		
		public T Get<T>(object documentId)
		{
			return Connection.TransactionGet<T>(this, documentId);
		}

		public void Update<T>(T document)
		{
			Connection.TransactionUpdate(this, document);
		}

		public void DeleteDocument<T>(T document)
		{
			Connection.TransactionDeleteDocument(this, document);
		}
		

		public void DeleteDocumentById(object documentId)
		{
			Connection.TransactionDeleteDocumentById(this, documentId);
		}

		public void Commit()
		{
			m_transactionCompletionHandled = true;
			Connection.CommitTransaction(this);
		}

		public void Rollback()
		{
			m_transactionCompletionHandled = true;
			Connection.RollbackTransaction(this);
		}

		public void Dispose()
		{
			if (!m_transactionCompletionHandled)
			{
				Rollback();
			}
		}
	}
}