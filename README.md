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
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

[![PoW Proxy][product-screenshot]](https://github.com/SparkyCoder/ProofOfWorkProxy)

The PoW Proxy is a practical way to monitor your miners. 

Do you:
* Want to see realtime connections of your miners?
* (coming soon) Want to be notified when a miner gets disconnected?
* Want to see realtime data transfer statistics between your miners and pool?
* Perhaps you want to debug your miner and view the raw Json-RPC communications?

Then this simple console application will help you. 

Unlike other proxy applications, PoW Proxy has no dev fees nor will it ever add any. The proxy will stay completely free and open source.

<p align="right">(<a href="#top">back to top</a>)</p>



## Technical Details

The application was build with speed at the forefront of it's developement. Each miner will be given two threads. One for miner to pool communication and another for pool to miner data transfer. This will create minimal latency and ensure your submitted shares don't become stale. 

The PoW Proxy supports Stratum protocols V1 and V2, but does not support SSL connections at this time. 

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

There are three ways to start using PoW Proxy
* With a GPU miner on the same computer (easiest)
* With ASICs on your Local Area Network (moderate difficulty)
* Over the internet on a static IP or domain (most difficult)

### Prerequisites

First, identify the IP address of your proxy. 
* Tip: if you're using a GPU miner on the same machine the IP address you're looking for is ```127.0.0.1```
* If you're using ASICs on the same network then you'll want to open a command prompt (Windows + R and type 'cmd') and enter the following
  ```sh
  ipconfig
  ```
  you'll want to take note of your ```IPv4 Address```. It will look something like 192.168.1.xxx. 

  Once you have this information, open up your miner's configuration and replace your mining pool address with the proxy IP.  

  For example:
  Replace ```stratum2+ssl://eu.crazypool.org:3333``` with ```stratum2+ssl://127.0.0.1:6673```

  As you can see above the default port number is 6673. However, if you want to change this, you can locate a file called ```App.config``` under the ```\ProofOfWorkProxy``` folder and update the ```ProxyPort``` value accoordingly.

  All done! You should be ready to go.



<!-- USAGE EXAMPLES -->
## Usage

Make sure your PoW Proxy application is running and says ```Waiting for connections......``` <br>
Start your miner and you'll see the statistics start displaying. 

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- ROADMAP -->
## Roadmap

- [x] Add concurrent connections
- [x] Support debug mode and statistics overview mode
- [ ] Add notifications for disconnected miners
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