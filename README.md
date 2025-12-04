# upgradable-market-system
🛒 Market System (Unity)
Overview

This project shows the main building blocks of a market system in a game. Player can see new items their prices and can purchase items with the in game money and upgrade the items. The system is based on an event driven system that uses data-runtime difference, UI updates, save/load systems.

Features

SO based data base

Runtime item state (unlocked, current level, owned amount etc.)

Dynamic UI generation (market items are automatically generated)

Purchase control (insufficient funds → error SFX, sufficient founds → purchase completion, SFX)

Event-driven architecture

OnMarketItemClicked

Player progression is saved and loaded with JSON system

![Image](https://github.com/user-attachments/assets/b19379f4-8718-401a-a98a-5fad4a0da3ac)

🛠 Technical Info

Unity Version: 2022.3.x
Platform: Mobile
Technologies used: ScriptableObjects, Event System, JSON save, Dynamic UI
