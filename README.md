# Unity Design Extensions (UDE)
Every developer is a work juggling multitasker, it's always the case, but should it have to be?

*UDE* contains a collection of useful tools such as scripts, design frameworks, and controls to enhance the developer's experience, it eliminates the distractions and centralizes your focus on the priorities. Focus on what you actually want to do, leaving the tedious, time-consuming bits to us.

## Overview

### [State Model Framework](Assets/CoreCollections/Framework/SMF.md)
Provides a clear workflow structure for your project which puts you in control over scalability, makes it easy for you to expand and debug your project.

### Menu System ```in progress```
Enables you to easily navigate through your menu pages/popups with transitions and animation (using unity's state machine) of your choice without writing any code.

### [Event System](Assets/CoreCollections/Framework/Event/EVENT.md)
Allows you to assign events during runtime via reflection it's fast and well optimized for **all platforms** including mobile platforms. Can also support multiple parameters unlike the unity event system with single parameters.

## What's next?
State Model Framework combined with the Event System allows you to create generic components which makes it reusable in other projects, regardless of its classification. Therefore, we encourage you all to share your work in the community so that you and others can benefit from. Check out our [community page](https://bitbucket.org/pepupstudios/community-components/src/master/).

We would love to see what you guys can create, Good Luck!

## FAQs
### What does core repository contain?
State model framework with event system and state conditions.

### Is it compatible with my game?
It's a one size fits all! *UDE* tools are extensible, making it compatible with all gaming genres/platforms including yours.

### Is it optimized?
Yes, these scripts have been inspired from many sources, combined and improved over time.

### What are the advantages of Event System over Unity Event System?
Event System uses reflection which allows developers to register/unregister to events during runtime. Moreover, it supports multiple parameters and a custom editor script, which other developers can build among.

### Will the State Model Framework benefit me?
It is a standard model framework which provides developers with a proper and easy structure for their workflow. It defines clear guidelines that divide behaviors into states, which team members can easily follow and extend upon. Read more about it over [here]().

### When will the Menu System be available?
It will take few months before the final draft could be released.

## Authors

* **Faraz** - *Initial work* - [1Faraz](https://github.com/1Faraz)

See also the list of [contributors](https://github.com/PepUpStudios/unity-design-extensions/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

* [Dan](danishmalik.maplesoftwares@gmail.com), thanks for making this happen.
