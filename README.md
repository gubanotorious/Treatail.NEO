<p align="center">
  <img src="Treatail.png" />
</p>

## This Repository
This repository contains the source code for the Treatail Smart Contract for the NEP5 Token (TTL) and Treatail Asset management, core middleware (leveraging [neo-lux](https://github.com/cityofzion/neo-lux)), REST API middleware, test libraries, and test harness.  It also contains a preliminary proof of concept Smart Contract that will be used by the product in the future to facilitate deal escrow payment distribution and supply chain workflows for deal fulfillment.

## The Product / dApp
### Overview
Treatail is a revolutionary commerce layer that sits on top of all the sites and marketplaces on the Internet.  Our platform allows for buyers, both consumer and business, to make offers and receive personalized deals on goods and services listed for sale on any site doing commerce on the Internet.  Our goal is to help buyers to get unpublished, personalized deals and help online retailers boost profits by optimizing inventory through precision discounts and reduced cart abandonment.

The cornerstone of the next evolution to the Treatail Ecosystem will be the creation of Treatail Token (TTL) and smart contracts to facilitate transactions on the blockchain and to further incent users.  To date, blockchain has primarily been available to technical users.  Treatail aims to change this by bringing the power of the NEO blockchain to the average user using intuitive interfaces and seamless integration into the Treatail platform.

Find out more at http://www.treatail.com and check out the whitepaper below.

### Demo Build
There is a demo build of this code hosted at https://neo.treatail.com.              
_You must use this URL when signing up and logging in.  If you use the standard Treatail website signup and login, you will be in the production site without this code deployed and you will end up with the wrong extension build installed.  If this happens, just right click and uninstall the extension from chrome and go to the correct url._

The extension you want to have installed is this version: https://chrome.google.com/webstore/detail/treatail/panhggdaegfhikjdbahbbgdoefoljjjl

Simply sign up for an account with your e-mail address to login as a buyer.  For the demo, the only store that offers can be made to is the "Treatail Demo Store".  We will respond to all offers as the seller, but if you need to gain access to the seller interface, please e-mail me or find me on the NEO Smart Economy Discord [@gubanotorious](https://discord.gg/zRq6Jba).

### Documentation
#### Whitepaper
- A brief whitepaper for the Treatail product and blockchain offering can be found here:
https://drive.google.com/open?id=1UlcbIkmbPJnSHAFiY1V20TCX4s2XMsQO8NVwWSpfcnY

#### Features and User Guide
- A Feature and User Guide for the functionality offered by Treatail
https://drive.google.com/open?id=1bBZodhVxSy06pFpWvwerCgalxyYjhkymXMzGmIk2Lkw

#### Code and API Documentation 
- Commented code w/Visual Studio XML comments
- Additional compiled Microsoft HTML Help file (.chm) found in the /Docs folder

#### Videos
- Quick intro video
https://youtu.be/tWQ0baMSSr8
- Video walkthrough basic blockchain based functionality (more detail in the user manual / whitepaper)
https://youtu.be/Fcz8XJctHok

### Architecture and Hosting
#### Hosting
Treatail is hosted on Microsoft Azure - Platform as a Service

#### Frontend
Microsoft .NET Web Application and Mobile Apps available for iOS and Android

#### Backend
Microsoft .NET WebAPI (REST), Treatail.NEO Middleware

#### Databases
Microsft SQL Server, Redis, and Microsoft Cosmos DB

#### Storage
Azure Blob Storage, NEO Blockchain

### ToDo / Roadmap
- Integrate Treatail Token and Treatail Asset functionality into Treatail Mobile apps
- Complete escrow workflow contract testing and development
- Build escrow workflow tests
- Build escrow and workflow features into Treatail app(s)

## Credits
This project wouldn't have been possible without the support from the [City of Zion](https://github.com/CityOfZion) NEO development community.




