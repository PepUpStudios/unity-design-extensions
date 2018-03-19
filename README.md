# Unity Design Extensions (UDE)
Every developer is a work juggling multitasker, it's always the case, but should it have to be?

*UDE* contains a collection of useful tools such as scripts, design frameworks, and controls to enhance the developer's experience, it eliminates the distractions and centralizes your focus on the priorities. Focus on what you actually want to do, leaving the tedious, time-consuming bits to us.

## Overview

### Core Framework
#### [State Model](Assets/Core%20Collections/Framework/README.md)
Provides a clear workflow structure for your project which puts you in control over scalability. Also makes it easy for you to maintain and debug your project.

#### [Event System](Assets/Core%20Collections/Framework/Event/README.md)
Allows you to assign events during runtime via reflection it's fast and well optimized for **all platforms** including mobile platforms. Can also support multiple parameters unlike the unity event system with single parameters.

#### [State Condition](Assets/Core%20Collections/Framework/Conditions/README.md)
It's an open-end plugin that transitions between states using specific conditions. This makes [State Model Framework](Assets/Core%20Collections/Framework/README.md) (SMF) flexible since various state condition scripts could be attached to a single game object.

### Community Extensions
Our community is filled with talented people willing to help others out.

The extensions mentioned below are made using the core framework:

## What's next?
State Model Framework combined with the Event System allows you to create generic components which makes it reusable in other projects, regardless of its classification. Therefore, we encourage you all to share your work in the community so that you and others can benefit from. Check out our [community page](Assets/Community%20Extensions).

We would love to see what you guys can create, Good Luck!

## Contributing
Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## FAQs
### What does core repository contain?
[State model](Assets/Core%20Collections/Framework/README.md), [event system](Assets/Core%20Collections/Framework/Event/README.md), and [state conditions](Assets/Core%20Collections/Framework/Conditions/README.md).

### Is it compatible with my game?
It's a one size fits all! *UDE* tools are extensible, making it compatible with all gaming genres/platforms including yours.

### Is it optimized?
Yes, these scripts have been inspired from many sources, combined and improved over time. It's light and works on all platforms including mobile platforms.

### What are the advantages of *UDE* Event System over Unity Event System?
Event System uses reflection which allows developers to register/unregister to events during runtime. Moreover, it supports multiple parameters and a custom editor script, which other developers can build upon.

### Will the State Model Framework benefit me?
It is a standard model framework which provides developers with a proper and easy structure for their workflow. It defines clear guidelines that divide behaviors into states, which team members can easily follow and extend upon. Read more about it over [here](Assets/Core%20Collections/Framework/SMF.md).

### When will the Menu System be available?
It will take few months before the final draft could be released.

## Authors

* **Faraz** - *Initial work* - [1Faraz](https://github.com/1Faraz)

See also the list of [contributors](https://github.com/PepUpStudios/unity-design-extensions/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

* [Dan](@danishmalik.maplesoftwares@gmail.com), thanks for making this happen.
