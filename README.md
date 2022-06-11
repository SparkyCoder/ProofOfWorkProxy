<div id="top"></div>


[![Stargazers][stars-shield]][stars-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">
  <h3 align="center">PoW Proxy</h3>

  <p align="center">
    A practical solution to monitor your Proof of Work miners
    <br />
    <a href="https://github.com/SparkyCoder/ProofOfWorkProxy/blob/main/README.md"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/SparkyCoder/ProofOfWorkProxy/issues">Report Bug</a>
    ·
    <a href="https://github.com/SparkyCoder/ProofOfWorkProxy/issues">Request Feature</a>
  </p>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#technical-details">Technical Details</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#step-1-configure-your-proxy-settings">Step 1 (Configure your Proxy Settings)</a></li>
        <a href="#step-2-update-your-miner-settings">Step 2 (Update your Miner Settings)</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li> 
    <li><a href="#troubleshooting">Troubleshooting</a></li> 
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

[![PoW Proxy][product-screenshot]](https://github.com/SparkyCoder/ProofOfWorkProxy)

Introducing PoW Proxy, from the developer that brought you [MyAdaRewards.com](https://www.myadarewards.com/) and the [MyAdaRewards Chrome Extension](https://chrome.google.com/webstore/detail/my-ada-rewards/ohgbmofhglhmjpjeconlmlkekoajlabb).

The PoW Proxy is a practical way to monitor and gather statistics on your miners. 

Do you:
* Want to see realtime connections of all your miners in one spot?
* Want to be notified when a miner is down or disconnected?
* Want to see realtime statistics of your miner and pool communications?
* Perhaps you want to troubleshoot your miner and view the raw Json-RPC communications?

Then this simple console application is for you. 

Unlike other proxy applications, PoW Proxy promises no dev fees will ever be added. This proxy will stay completely free and open source.

<p align="right">(<a href="#top">back to top</a>)</p>



## Technical Details

PoW Proxy was build with speed at the forefront of it's developement. Each miner will be given two processing threads. One for miner ---> pool communication and another for pool ---> miner data transfer. This will create minimal latency and ensure your submitted shares don't become stale. 

The PoW Proxy supports Stratum protocols V1 and V2, but does not support SSL connections at this time.

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

There are three ways to start using PoW Proxy
* With a GPU miner on the same computer (easiest)
* With a GPU or ASIC on your Local Area Network (moderate difficulty)
* Over the internet on a static IP or domain (most difficult)

### Prerequisites

Let's get your configuration working first. There are only two steps!

### Step 1 (Configure your Proxy Settings)

Navigate to the `App.config` file in the PoW PRoxy directory. Now edit the file in a text editor. You should see [something like this](https://raw.githubusercontent.com/SparkyCoder/ProofOfWorkProxy/main/ProofOfWorkProxy/App.config). 

Update the values in `MiningPoolDomain` and `MiningPoolPort` to match your pool. 

For example, if your miner settings currently look like this: 
```
stratum2+ssl://eu.crazypool.org:3333 -u userName -p passwordHere
```
the you'll want to update your App.config to look like this:

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <add key="ProxyPort" value="6673"/>
    <add key="MiningPoolDomain" value="eu.crazypool.org"/> 
    <add key="MiningPoolPort" value="3333"/> 
    <add key="DebugOn" value="false"/>
  </appSettings>
</configuration>
```

First step done!

### Step 2 (Update your Miner Settings)

For this step we'll want to update your miner with the IP and Port of your new proxy. 

For example, if your miner looked like this:

```
stratum2+ssl://eu.crazypool.org:3333 -u userName -p passwordHere
```

We'll want to update it to:

```
stratum2+ssl://YourProxyIpAddressHere:6673 -u userName -p passwordHere
```

Not sure how to find your IP address?

* Tip: if you're using a GPU miner on the same machine the IP address you're looking for is `127.0.0.1`
```
stratum2+ssl://127.0.0.1:6673 -u userName -p passwordHere
```
* If you're using a GPU or ASIC on the same network (LAN) then you'll want to open a command prompt (Windows + R and type 'cmd') and enter the following
  ```
  ipconfig
  ```
  you'll want to take note of your `IPv4 Address`. It will look something like 192.168.1.xxx. 

  Once you have this IP, update your miner settings as shown above.

  All done! You should be ready to go.



<!-- USAGE EXAMPLES -->
## Usage

Make sure your PoW Proxy application is running and says ```Waiting for connections......``` <br>
Start your miner and you'll see the statistics start displaying. 

If you want to troubleshoot your miner or proxy and see more verbose messages, you can use the debug flag. That setting is in the same file you updated earlier, the [App.Config](https://raw.githubusercontent.com/SparkyCoder/ProofOfWorkProxy/main/ProofOfWorkProxy/App.config). 

This time however, you'll want to update the `DebugOn` flag from `false` to `true`. 

```
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <add key="ProxyPort" value="6673"/>
    <add key="MiningPoolDomain" value="eu.crazypool.org"/>
    <add key="MiningPoolPort" value="3333"/>
    <add key="DebugOn" value="true"/> 
  </appSettings>
</configuration>
```

Now you'll start see raw Json-RPC messages flow through. 

[![Debug PoW Proxy][debug-screenshot]](https://github.com/SparkyCoder/ProofOfWorkProxy)

<p align="right">(<a href="#top">back to top</a>)</p>

## Troubleshooting

<b>Issue</b>: Screen is black with no information. <br/>
<b>Reason</b>: It's possible you held the scroll bar or put focus on the console display with your cursor. <br/>
<b>Fix</b>: Click on the PoW Proxy console application and hit `Enter` on your keyboard. Messages should continue. 

<p align="right">(<a href="#top">back to top</a>)</p>

<!-- ROADMAP -->
## Roadmap

- [x] Add multi-threaded TCP Socket connections
- [x] Support debug mode and statistics overview mode
- [ ] Add notifications for disconnected miners
- [ ] Add additional statistics (verified shares, error details, etc...)
- [ ] Support SSL connections

See the [open issues](https://github.com/SparkyCoder/ProofOfWorkProxy/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#top">back to top</a>)</p>


<!-- LICENSE -->
## License

Distributed under the BSD-3-Clause license. See `LICENSE.txt` for more information.

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- CONTACT -->
## Contact

SparkyCoder - [Email contact](mailto:got-sparky@live.com) 

Project Link: [https://github.com/SparkyCoder/ProofOfWorkProxy](https://github.com/your_username/repo_name)

<p align="right">(<a href="#top">back to top</a>)</p>


[stars-shield]: https://img.shields.io/github/stars/SparkyCoder/ProofOfWorkProxy?style=for-the-badge
[stars-url]: https://github.com/SparkyCoder/ProofOfWorkProxy/stargazers
[license-shield]: https://img.shields.io/github/license/SparkyCoder/ProofOfWorkProxy?style=for-the-badge
[license-url]: https://raw.githubusercontent.com/SparkyCoder/ProofOfWorkProxy/main/LICENSE
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]:https://www.linkedin.com/in/david-kobuszewski-60315428/
[product-screenshot]: https://raw.githubusercontent.com/SparkyCoder/ProofOfWorkProxy/main/Images/screenshot.png
[debug-screenshot]: https://raw.githubusercontent.com/SparkyCoder/ProofOfWorkProxy/main/Images/DebugScreenshot.png