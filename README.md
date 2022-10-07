# TCP Client Example

```csharp
TcpClient client = new TcpClient();
client.Connect("127.0.0.1", 9900);

using(NetworkStream networkStream = client.GetStream())
{
    byte[] encodedTime = new byte[sizeof(long)];
    networkStream.Read(encodedTime, 0, encodedTime.Length);
    
    Console.WriteLine($"{BitConverter.ToInt64(encodedTime, 0)}");
}
```
