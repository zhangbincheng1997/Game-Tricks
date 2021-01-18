#include <enet/enet.h>
#include <msgpack.hpp>
#include <iostream>
using namespace std;

#define HOST "127.0.0.1"
#define PORT 1234
#define OK 0

ENetHost *createServer()
{
    ENetAddress address;
    ENetHost *server;

    enet_address_set_host(&address, HOST);
    address.port = PORT;

    server = enet_host_create(
        &address /* the address to bind the server host to */,
        32 /* allow up to 32 clients and/or outgoing connections */,
        2 /* allow up to 2 channels to be used, 0 and 1 */,
        0 /* assume any amount of incoming bandwidth */,
        0 /* assume any amount of outgoing bandwidth */
    );

    if (server == NULL)
    {
        printf("An error occurred while trying to create an ENet server host.\n");
        exit(EXIT_FAILURE);
    }

    printf("SERVER OK!\n");
    return server;
}

int main(int argc, char **argv)
{
    if (enet_initialize() != 0)
    {
        printf("An error occurred while initializing ENet.\n");
        exit(EXIT_FAILURE);
    }
    atexit(enet_deinitialize);

    ENetHost *server = createServer();

    ENetEvent event;

    int COUNT = 0;

    while (true)
    {
        /********** 无阻塞 **********/
        while (enet_host_service(server, &event, 0) > 0)
        {
            switch (event.type)
            {
            case ENET_EVENT_TYPE_CONNECT:
                printf("A new client connected from %x:%u.\n",
                       event.peer->address.host,
                       event.peer->address.port);
                event.peer->data = (void *)"Client information";
                break;
            case ENET_EVENT_TYPE_RECEIVE:
                // printf("A packet of length %u containing %s was received from %s on channel %u.\n",
                // 	   event.packet->dataLength,
                // 	   event.packet->data,
                // 	   event.peer->data,
                // 	   event.channelID);
                ++COUNT;
                enet_peer_send(event.peer, 0, event.packet);
                break;
            case ENET_EVENT_TYPE_DISCONNECT:
                printf("%s disconnected.\n", event.peer->data);
                event.peer->data = NULL;
                break;
            }

            printf("connectedPeers: %d, totalReceived: %d \n", server->connectedPeers, COUNT);
        }
    }

    enet_host_destroy(server);
    return OK;
}