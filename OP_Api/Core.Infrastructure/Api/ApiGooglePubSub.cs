using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Grpc.Auth;
using Grpc.Core;

namespace Core.Infrastructure.Api
{
    public class ApiGooglePubSub
    {
        private const string Project = "gam-project-cgd-x0l-zm4";
        private const string Topic = "dlex";
        private const string SubscriptionId = "YOUR SUBSCRIPTION ID";
        public async Task<int> PublishToTopic()
        {
            // First create a topic.
            GoogleCredential cred = GoogleCredential.FromFile("dev-dlex-pub.json");
            Channel channel = new Channel(
                PublisherServiceApiClient.DefaultEndpoint.Host, PublisherServiceApiClient.DefaultEndpoint.Port, cred.ToChannelCredentials());
            PublisherServiceApiClient client = PublisherServiceApiClient.Create(channel);
            //
            //PublisherServiceApiClient publisherServiceApiClient = await PublisherServiceApiClient.CreateAsync();
            // Initialize request argument(s)
            //
            var message = new PubsubMessage()
			{
				Data = ByteString.CopyFromUtf8("Hi Team!")
			};
            //
            PublishRequest request = new PublishRequest
            {
                TopicAsTopicName = new TopicName(Project, Topic),
                Messages =
                {
                    message,
                },
            };
            // Make the request
            PublishResponse response = await client.PublishAsync(request);
            // Shutdown the channel when it is no longer required.
            //channel.ShutdownAsync().Wait();
            return 0;
        }
    }
}
