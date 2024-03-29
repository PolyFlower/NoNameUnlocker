﻿using System;
using System.Collections.Generic;
using System.Threading;

public class ServerMessageHandler
{
	public static void HandleServerKeyMessage(BinaryMessageReader binaryReaderWrapper, Server server)
	{
		binaryReaderWrapper.ReadBytes(8);
	}

	public static void HandleServerAuthStatusMessage(BinaryMessageReader binaryReaderWrapper, Server server)
	{
		server.authStatusFlag = binaryReaderWrapper.ReadUInt16();
		server.authToken = binaryReaderWrapper.ReadBytes(16);
		new Thread(new ParameterizedThreadStart(server.action_7.Invoke)).Start(server.authStatusFlag);
	}

	public static void HandleServerWardenUploadMessage(BinaryMessageReader binaryReaderWrapper, Server server)
	{
		ConsoleColor consoleColor = (ConsoleColor)binaryReaderWrapper.ReadByte();
		ushort num = binaryReaderWrapper.ReadUInt16();
		string text = binaryReaderWrapper.ReadString((int)num);
		ConsoleColor foregroundColor = Console.ForegroundColor;
		Console.ForegroundColor = consoleColor;
		Logger.Info(text);
		Console.ForegroundColor = foregroundColor;
	}

	public static void HandleServerBroadcastMessage(BinaryMessageReader binaryReaderWrapper, Server server)
	{
		ConsoleColor color = (ConsoleColor)binaryReaderWrapper.ReadByte();
		ushort length = binaryReaderWrapper.ReadUInt16();
        string message = binaryReaderWrapper.ReadString((int)length);
        ConsoleColor foregroundColor = Console.ForegroundColor;
		Console.ForegroundColor = color;
		Logger.Info(message);
		Console.ForegroundColor = foregroundColor;
	}

	public static void HandleServerPayloadMessage(BinaryMessageReader binaryReaderWrapper, Server server)
	{
		int length = binaryReaderWrapper.ReadInt32();
		server.aPayload = binaryReaderWrapper.ReadBytes(length);
		length = binaryReaderWrapper.ReadInt32();
		server.luaPayload = binaryReaderWrapper.ReadBytes(length);
        server.action_3?.Invoke();
    }

	public static void HandleServerNeedlePayloadMessage(BinaryMessageReader binaryReaderWrapper, Server server)
	{
		int num = binaryReaderWrapper.ReadInt32();
		byte[] array = binaryReaderWrapper.ReadBytes(num);
		int num2 = binaryReaderWrapper.ReadInt32();
		if (server.action_4 != null)
		{
			server.action_4(array, num2);
		}
	}

	public static void HandleServerHookPayloadMessage(BinaryMessageReader binaryReaderWrapper, Server server)
	{
		int num = binaryReaderWrapper.ReadInt32();
		byte[] array = binaryReaderWrapper.ReadBytes(num);
		int num2 = binaryReaderWrapper.ReadInt32();
		if (server.action_5 != null)
		{
			server.action_5(array, num2);
		}
	}

	public static void HandleServerOffsetsMessage(BinaryMessageReader binaryReaderWrapper, Server server)
	{
		uint num = binaryReaderWrapper.ReadUInt32();
		server.list_0 = new List<ulong>();
		int num2 = 0;
		while ((long)num2 < (long)((ulong)num))
		{
			server.list_0.Add(binaryReaderWrapper.ReadUInt64());
			num2++;
		}
		if (server.action_1 != null)
		{
			server.action_1();
		}
	}

	public static void HandleServerToolOffsetsMessage(BinaryMessageReader binaryReaderWrapper, Server server)
	{
		uint num = binaryReaderWrapper.ReadUInt32();
		server.list_1 = new List<ulong>();
		int num2 = 0;
		while ((long)num2 < (long)((ulong)num))
		{
			server.list_1.Add(binaryReaderWrapper.ReadUInt64());
			num2++;
		}
		if (server.action_2 != null)
		{
			server.action_2();
		}
	}

	public static void HandleServerRequestGameModuleMessage(BinaryMessageReader binaryReaderWrapper, Server server)
	{
		binaryReaderWrapper.ReadUInt32();
		server.action_6();
	}

    [Obsolete("Method usage is unknown")]
	public static void HandleServerUnknownMessage(BinaryMessageReader binaryReaderWrapper, Server server)
	{
		uint num = binaryReaderWrapper.ReadUInt32();
		byte[] array = binaryReaderWrapper.ReadBytes(16);
		byte[] array2 = binaryReaderWrapper.ReadBytes((int)num);
		if (server.action_8 != null)
		{
			server.action_8(array, array2);
		}
	}
}
