# Unity Design Extensions (UDE)
Every developer is a work juggling multitasker, it's always the case, but should it have to be?

UDE contains a collection of useful tools such as scripts, design frameworks, and controls to enhance the developer's experience, it eliminates the distractions and centralizes your focus on the priorities. Focus on what you actually want to do, leaving the tedious, time-consuming bits to us.

## Overview

### Core Framework
#### [State Model](Assets/Core%20Collections/SMF/README.md)
Provides a clear workflow structure for your project which puts you in control over scalability. Also makes it easy for you to maintain and debug your project.

#### [State Event](Assets/Core%20Collections/SMF/State%20Event/README.md)
Allows you to assign events during runtime via reflection it's fast and well optimized for **all platforms** including mobile platforms. Can also support multiple parameters unlike the unity event system with single parameters.

#### [State Condition](Assets/Core%20Collections/SMF/State%20Condition/README.md)
It's an open-end plugin that transitions between states using specific conditions. This makes [State Model Framework](Assets/Core%20Collections/SMF/README.md) (SMF) flexible and extremely accessible.

### Core System
#### [Inventory System](Assets/Core%20Collections/Systems/Inventory/README.md)
Data is categorized and stored as hash maps which makes retrieving it easy and fast than conventional inventory system. Keep in mind, this system is designed to deal with large data to achieve optimal read and write speeds during runtime.

#### [Menu System](Assets/Core%20Collections/Systems/Menu/README.md) ```Beta```
Enables you to easily navigate through your menu pages/popups with transitions and animation (using unity's state machine) of your choice without writing any code.

#### [Pool System](Assets/Core%20Collections/Systems/Pool/README.md)
Object pooling can offer a significant performance boost; it is most effective in situations where the cost of initializing a class instance is high, the rate of instantiation of a class is high, and the number of instantiations in use at any one time is low.

*More systems implementations coming soon!*

### Community Extensions
Our [community](Assets/Community%20Extensions) is filled with talented people willing to help others out. All of the extensions mentioned below are made using the core framework.

------------------------------------
#### Components
<table class="tg">
  <tr>
    <td class="tg-baqh" colspan="7" width="1000"><a href="Assets/Community%20Extensions/Components/Post Processing"><b>Post Processing</b></a></td>
  </tr>
  <tr>
    <td class="tg-9hbo" colspan="1">Volume</td>
    <td class="tg-9hbo" colspan="6">Vignette</td>
  </tr>
  <tr>
    <td class="tg-baqh" colspan="7"><a href="Assets/Community%20Extensions/Components/Health"><b>Health</b></a></td>
  </tr>
  <tr>
    <td class="tg-9hbo" colspan="7" width="25%">Standard Health</td>
  </tr>
</table>

------------------------------------
#### Controllers
<table class="tg">
  <tr>
    <td class="tg-baqh" colspan="7" width="1000"><a href="Assets/Community%20Extensions/Controllers/Detect"><b>Detection</b></a></td>
  </tr>
  <tr>
    <td class="tg-9hbo" colspan="1">Inputs</td>
    <td class="tg-9hbo" colspan="6">Objects</td>
  </tr>
  <tr>
    <td class="tg-baqh" colspan="7"><a href="Assets/Community%20Extensions/Controllers/Game%20Objects"><b>Game Object</b></a></td>
  </tr>
  <tr>
    <td class="tg-9hbo" colspan="1">Transform</td>
    <td class="tg-9hbo" colspan="6">GameObject</td>
  </tr>
  <tr>
    <td class="tg-baqh" colspan="7"><a href="Assets/Community%20Extensions/Controllers/Particles"><b>Particles</b></a></td>
  </tr>
  <tr>
    <td class="tg-9hbo" colspan="7">AutoDestructParticle</td>
  </tr>
  <tr>
    <td class="tg-baqh" colspan="7"><a href="Assets/Community%20Extensions/Controllers/Sprites/Color"><b>Sprites</b></a></td>
  </tr>
  <tr>
    <td class="tg-9hbo" colspan="7">Color</td>
  </tr>
  <tr>
    <td class="tg-baqh" colspan="7"><a href="Assets/Community%20Extensions/Controllers/UI/Canvas%20Group"><b>UI</b></a></td>
  </tr>
  <tr>
    <td class="tg-9hbo">Canvas Group</td>
    <td class="tg-9hbo">Effects</td>
    <td class="tg-9hbo">Image</td>
    <td class="tg-9hbo">Rect Transform</td>
    <td class="tg-9hbo">Slider</td>
    <td class="tg-9hbo">Text</td>
    <td class="tg-9hbo">UIEvent</td>
  </tr>
</table>

------------------------------------
#### Core Extensions
<table class="tg">
  <tr>
    <td class="tg-baqh" colspan="7" width="1000"><b>Coming soon...</b></td>
  </tr>
</table>

------------------------------------
## What's next?
State Model Framework combined with the Event System allows you to create generic components which makes it reusable in other projects, regardless of its classification. Therefore, we encourage you all to share your work in the community so that you and others can benefit from - Check out our [community page](Assets/Community%20Extensions).

We would love to see what you guys can create, Good Luck!

## Contributing
Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## FAQs
### What does core repository contain?
[State model](Assets/Core%20Collections/SMF/README.md), [event system](Assets/Core%20Collections/SMF/State%20Event/README.md), and [state conditions](Assets/Core%20Collections/SMF/State%20Condition/README.md).

### Is the framework compatible with my game?
It's a one size fits all! the scripts are extensible, making it compatible with all gaming genres/platforms including yours.

### Is the framework optimized?
Yes, these scripts have been inspired from many sources, combined and improved over time. It's light and works on all platforms including mobile platforms.

### What are the advantages of UDE Event System over Unity Event System?
UDE Event System uses reflection which allows developers to register/unregister to events during runtime. Moreover, it supports multiple parameters and a custom editor script, which other developers can build upon.

### Will the State Model Framework benefit me?
It is a standard model framework which provides developers with a proper and easy structure for their workflow. It defines clear guidelines that divide behaviors into states, which team members can easily follow and extend upon. Read more about it over [here](Assets/Core%20Collections/SMF/README.md).

### When will the Menu System be available?
It will take few months before the final draft could be released.

## Authors

* **Faraz** - *Initial work* - [1Faraz](https://github.com/1Faraz)

See also the list of [contributors](https://github.com/PepUpStudios/unity-design-extensions/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

* [Dan](danishmalik.maplesoftwares@gmail.com), thanks for making this happen.
