﻿namespace Castle.RabbitMq.WindsorIntegration.Tests
{
    using System.Collections.Generic;
    using System.Threading;
    using Castle.RabbitMq;
    using Castle.RabbitMq.IntegrationTests.Scenarios;
    using Castle.RabbitMq.Messaging;
    using FluentAssertions;
    using MicroKernel.Registration;
    using Windsor;
    using Xunit;


    public class MessageHandlerTest : IClassFixture<ChannelFixture>
    {
        public MessageHandlerTest(ChannelFixture channelFix)
        {
            this.Channel = channelFix.Channel;
        }

        public IRabbitChannel Channel { get; set; }

        // What_When_Should

        [Fact]
        public void MessageSentThroughBus_Should_ActivateMessageHandler()
        {
//            Purge("test_q_routing2");

            // Set initial state
            MyRoutableMessageHandler.Handled = false;

            const string vhost = "castle";
            var container = new WindsorContainer();

            var config = new ConfigSettings()
            {
                OurScope = "test_bus",
                VHost = vhost,
                ExchangeNamePrefix = "test_e_",
                QueueNamePrefix = "test_q_",
                NamespaceExchangeMapping = new Dictionary<string, string>()
                {
                    {"Castle.RabbitMq.WindsorIntegration.Tests", "test3"}
                }
            };

            container.Register(Component.For<ConfigSettings>().Instance(config));
            container.Install(new MessageHandlersInstaller(assembliesWithHandlers: typeof(MyRoutableMessageHandler).Assembly));

            var bus = container.Resolve<IBus>();
            bus.Send(new MyRoutableMessage() { RoutingKey = "routing2" });

            

            Thread.Sleep(0);

            MyRoutableMessageHandler.Handled.Should().BeTrue("the handler should be activated");
        }

        private void Purge(string queueName)
        {
            var queue = this.Channel.DeclareQueue(queueName, new QueueOptions()
            {
                AutoDelete = false,
                Durable = true,
                Exclusive = false
            });
            queue.Purge();
        }
    }
}