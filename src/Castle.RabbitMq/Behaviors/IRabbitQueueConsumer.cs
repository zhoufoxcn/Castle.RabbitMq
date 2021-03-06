﻿namespace Castle.RabbitMq
{
	using System;

	public interface IRabbitQueueConsumer
	{
		///	<summary>
		///	For	Rpc
		///	</summary>
		///	<typeparam name="TRequest"></typeparam>
		///	<typeparam name="TResponse"></typeparam>
		///	<param name="onRespond"></param>
		///	<param name="options"></param>
		Subscription Respond<TRequest, TResponse>(Func<MessageEnvelope<TRequest>, IMessageAck, TResponse> onRespond,
												  ConsumerOptions options);

		///	<summary>
		///	For	pure message consumption
		///	</summary>
		///	<param name="onReceived"></param>
		///	<param name="options"></param>
		Subscription Consume<T>(Action<MessageEnvelope<T>, IMessageAck>	onReceived,	
								ConsumerOptions	options);

		// MessageEnvelope<T> Receive<T>() where T : class;
		// MessageEnvelope<T> Peek<T>()	where T	: class;
	}

	public static class	RabbitQueueConsumerExtensions
	{
		// TODO: move this one to Extension	method
		// void	Consume<T>(Action<MessageEnvelope<T>> onMsgReceived, ConsumerOptions options) where	T :	class;

		public static Subscription Consume<T>(this IRabbitQueueConsumer	source,
											  Action<MessageEnvelope<T>, IMessageAck> onReceived)
		{
			return source.Consume<T>(onReceived, null);
		}

		public static Subscription Respond<TRequest, TResponse>(this IRabbitQueueConsumer source, 
			Func<MessageEnvelope<TRequest>,	IMessageAck, TResponse>	onRespond)
		{
			return source.Respond(onRespond, null);
		}
	}
}