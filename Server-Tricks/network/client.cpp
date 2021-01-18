#include <enet/enet.h>
#include <msgpack.hpp>
#include <stdio.h>
#include <stdlib.h>
#include <iostream>
using namespace std;

#define HOST "127.0.0.1"
#define PORT 1234
#define OK 0

bool isRunning = true;

struct Message
{
    int m_time;
    string m_data;
    MSGPACK_DEFINE(m_time, m_data);
};

ENetHost *createClient()
{
    ENetHost *client;

    client = enet_host_create(
        NULL /* create a client host */,
        1 /* only allow 1 outgoing connection */,
        2 /* allow up 2 channels to be used, 0 and 1 */,
        0 /* assume any amount of incoming bandwidth */,
        0 /* assume any amount of outgoing bandwidth */
    );

    if (client == NULL)
    {
        printf("An error occurred while trying to create an ENet client host.\n");
        exit(EXIT_FAILURE);
    }

    printf("CLIENT OK!\n");
    return client;
}

ENetPeer *createPeer(ENetHost *client)
{
    ENetAddress address;
    ENetPeer *peer;

    /* Connect */
    enet_address_set_host(&address, HOST);
    address.port = PORT;

    /* Initiate the connection, allocating the two channels 0 and 1. */
    peer = enet_host_connect(client, &address, 2, 0);
    if (peer == NULL)
    {
        printf("No available peers for initiating an ENet connection.\n");
        exit(EXIT_FAILURE);
    }

    printf("PEER OK!\n");
    return peer;
}

int main(int argc, char **argv)
{
    if (enet_initialize() != 0)
    {
        printf("An error occurred while initializing ENet.\n");
        exit(EXIT_FAILURE);
    }
    atexit(enet_deinitialize);

    ENetHost *client = createClient();

    ENetPeer *peer = createPeer(client);

    ENetEvent event;

    int COUNT = 0;

    /* Wait up to 5 seconds for the connection attempt to succeed. */
    if (enet_host_service(client, &event, 5000) > 0 && event.type == ENET_EVENT_TYPE_CONNECT)
    {
        printf("Connection to %s:%d succeeded.\n", HOST, PORT);
    }
    else
    {
        isRunning = false;
        /* Either the 5 seconds are up or a disconnect event was */
        /* received. Reset the peer in the event the 5 seconds   */
        /* had run out without any significant event.            */
        enet_peer_reset(peer);
        printf("Connection to %s:%d failed.\n", HOST, PORT);
    }

    while (isRunning)
    {
        /********** 无阻塞 **********/
        while (enet_host_service(client, &event, 0) > 0)
        {
            switch (event.type)
            {
            case ENET_EVENT_TYPE_RECEIVE:
            {
                // unpacking
                msgpack::object_handle oh = msgpack::unpack((char *)event.packet->data, event.packet->dataLength);
                msgpack::object obj = oh.get();
                auto res = obj.as<Message>();

                // data
                int m_time = res.m_time;
                string m_data = res.m_data;
                cout << "rtt: " << timeGetTime() - m_time << "ms" << endl;
                cout << "data: " << m_data << endl;

                enet_packet_destroy(event.packet);
                break;
            }
            case ENET_EVENT_TYPE_DISCONNECT:
                isRunning = false;
                printf("Connection failed!\n");
                break;
            }
        }

        if (isRunning)
        {
            Sleep(10); /* 模拟阻塞 */

            // data
            Message msg;
            msg.m_time = timeGetTime();
            msg.m_data = to_string(++COUNT);

            // packing
            stringstream buffer;
            msgpack::pack(buffer, msg);
            string data = buffer.str();

            ENetPacket *packet = enet_packet_create(data.data(), data.length(), ENET_PACKET_FLAG_RELIABLE);
            enet_peer_send(peer, 0, packet);
        }
    }

    enet_peer_disconnect(peer, 0);

    enet_host_destroy(client);
    return OK;
}