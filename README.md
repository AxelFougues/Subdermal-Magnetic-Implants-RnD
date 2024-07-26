# Current work
This is an archive of past work. If you're interested in what's happening now check these out:

## ZINC
A multitool app for NFC and magnetic implants, including haptics, analytics, gamification and experimentation:

- ZINC-The-Cyborg-Dashboard: https://github.com/AxelFougues/ZINC-The-Cyborg-Dashboard

## Lodestones
A suite of magnet implant stimulator devices made for everyday use and convenience. Used in conjunction with ZINC for exciting use-cases:

- Lodestones: https://github.com/AxelFougues/Lodestone-biomagnet-tools
- Dangerous Things, Lodestone PICO for sale announcement: https://forum.dangerousthings.com/t/lodestone-pico-now-available/21441/6
- Dangerous Things, Lodestone PICO product info post: https://forum.dangerousthings.com/t/lodestone-pico/21440

## Desktop AR/VR haptic feedback demo using a Leap Motion 2

- https://github.com/AxelFougues/Biomagnet-Leap-Motion-Haptic-Engine

# Past work

A collection of work on SMIs over the years including data, research, hardware and software development.
**Disclaimer: Nothing here is maintained or has been in years, some projects are unfinished and might even not run in newer environments. This repo is ment to be inspiration for future SMI tech. A ressource to look through when exploring SMIs. Please use my mistakes to anticipate yours.**

## Academic work
**Content of this repo**

Here you will find some resources extracted from a 6 month research work at CNRS IT lab LIRMM in Montpellier. This opportinity was given to me after presenting my home-research to Abderrahmane Kheddar (Haptics researcher). This work aims at better understanding the mechanics of "sensing", the feeling induced by magnetic fields to a person equiped with fingertip subdermal magnets.
In turn understanding and figuring out how to control this phenomenon allowed me to produce some interesting spatial haptic rendering as well as some texture and mechanical rendering such as pressing virtual buttons and distinguishing virtual surfaces.
There was an attempt at publishing such research but a combination of deadlines, life events, lack of test subjects other than myself and the difficulty of dealing with academic "ethical standards" resulted in no publication and many subjects left open to explore.




## SMIS: A unified ecosystem for SMI study and use
Designing consumer facing and easy to use tools for anallyzing and using SMIs in a unified and comparable manner. This project aims at creating both the devices and the mobile app that allow for a wide range of finger-magnet stimulation.
One core focus is to provide the implantees with a unified testing procedure for SMIs so that usable data can be produced in mass and therefore interesting analysis can be done both on the implant performace and the user's sensitivity.
A secondary goal is to discover and explore new uses for sensing by providing a convenient platform to do so. Some of these uses are sensory substitution through trainning, gamification of sensing (possibly spatial rendering of environments) and innovative human-machine interaction (notifications, gps directions... through sensing).

Project page: https://www.behance.net/gallery/122871797/SMIS-Haptics-VR-and-Biohacking

Device page: https://www.behance.net/gallery/123152077/SMIS-CUBE

Unity project on Github : https://github.com/AxelFougues/SMIS-App

Device design on Github : https://github.com/AxelFougues/SMIS-device-design

Wiki: https://github.com/AxelFougues/SMIS-device-design/wiki

### A mobile app as a core
The mobile app was implemented in unity and uses the devices audio channels to produce the signal for the devices as the sensing range and audio range of frequencies are similar. The app is meant to contain standardised tests of amplitude sensitivity, frequency sensityvity and more using various signal types. These procedures are user friendly and produce sharable results that can be compared with other results thanks to the standardization of the tests. The app also contains a signal generator for self-experimentation. Finally the app was meant to be a platform for a plethora of interactive uses of SMIs, demonstrating AR applications, mini games and message encoding.

### A suite of tiny handheld devices
I envisioned a selection of devices all following similar guidelines of portability, practicality and field consistency. They would use bluetooth to receive the app's output and have a small onboard amplification and coils. The design that got all the way through prototyping was the SMIS cube. A matchbox sized device with two independently controlled coils for then index finger and middle finger. For prototyping I used 3D printed casings, off the shelf amplification and axial coils. The design worked exactly as expected although the low quality components made it unreliable long term.
Other device ideas involved a credit card format of the cube that fits much better in the user's pocket. Some versions were applied to the back of a mobile device and were wire to reduce latency for AR applications. In some the finger coils were external and in the shape of rings. This meant that the device could be attatched to the users hand giving full freedom of movement. These later designs were inspired by the following BitSense project and went on to be used in following research for their practicality.

## BitSense Time Paradox
BitSense is in a way the ancestor to SMIS and was built with the premise of producing an induced SMI "sensing" maze game. Exploring the limitations of spatial hand/implant tracking and the rendering of spatial information to the implants through magnetic "sensing" signals. A POC of the game as well as a Toolkit were made in this project.

Project paage: https://www.behance.net/gallery/106134293/BitSense-Biohacking-a-sixth-digital-sense

The Toolkit + Game, Unity project on github: https://github.com/AxelFougues/BitSense-Toolkit-and-Time-Paradox

An augmented reality level visualizer for a third party observer, Unity project on github: https://github.com/AxelFougues/Visualizer-for-BitSense-Time-Paradox-levels
 
 
 ## Additional resources

Self published ebook "Getting to know the magnet inside you: A guide to magnetic implants" :
https://play.google.com/store/books/details?id=y07bDwAAQBAJ

Videos (SMIs, BitSense and Haptics):
https://youtube.com/playlist?list=PLYKu4vn6TpCEsDx0OuiTX4lcnOOtgskgy

Facebook community started in the same process where I shared some pictures and updates:
https://www.facebook.com/groups/BiohackingFrance

