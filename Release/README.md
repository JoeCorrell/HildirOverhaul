<div align="center">

# Hildir Trading Overhaul

Give Hildir the same premium trader UI style while keeping her vanilla shop stock unchanged.

[![Version](https://img.shields.io/badge/Version-0.0.1-blue?style=for-the-badge)](https://github.com/JoeCorrell/HildirOverhaul/releases)
[![BepInEx](https://img.shields.io/badge/BepInEx-5.4.2200+-orange?style=for-the-badge)](#requirements)
[![Catalog](https://img.shields.io/badge/Catalog-Vanilla-green?style=for-the-badge)](#features)

---

<h3>Buy</h3>
<p align="center">
<img src="https://raw.githubusercontent.com/JoeCorrell/HaldorOverhaul/main/Screenshots/Buy.png" alt="Buy Tab" width="600"/>
</p>

<hr/>

<h3>Sell</h3>
<p align="center">
<img src="https://raw.githubusercontent.com/JoeCorrell/HaldorOverhaul/main/Screenshots/Sell.png" alt="Sell Tab" width="600"/>
</p>

<hr/>

<h3>Bank</h3>
<p align="center">
<img src="https://raw.githubusercontent.com/JoeCorrell/HaldorOverhaul/main/Screenshots/Bank.png" alt="Bank Tab" width="600"/>
</p>

---

## Features

Hildir's buyable shop items and prices are not modified<br/>
Hildir now buys any item from your inventory<br/>
Sell values are configurable via `HildirOverhaul.hildir.sell.json`<br/>
Full controller support for tabs, lists, and actions<br/>
Shared bank balance across HaldorOverhaul, HildirOverhaul, and BogWitchOverhaul when all 3 mods are installed

<hr/>

## Bank

Hildir can be used as a personal banker:

`Deposit` moves coins from inventory into your bank balance<br/>
`Withdraw` moves coins from bank back to inventory<br/>
Buy and sell operations use the same bank flow automatically

<hr/>

## Compatibility Notes

This mod targets **Hildir only** by prefab name.

Other traders (Haldor, Bog Witch, and modded trader NPCs) continue using vanilla `StoreGui` unless their matching overhaul mod is installed<br/>
Mods that replace or heavily patch Hildir may conflict<br/>
Mods that intercept `StoreGui.Show` before this mod may block the custom UI

<hr/>

## Requirements

Valheim<br/>
BepInEx 5.4.2200 or newer

<hr/>

## Installation

Install BepInEx<br/>
Download the latest release<br/>
Extract to `BepInEx/plugins/HildirOverhaul/`<br/>
Copy `HildirOverhaul.hildir.sell.json` into `BepInEx/config/`<br/>
Launch the game

<hr/>

## Credits

Inspired by [TradersExtended](https://thunderstore.io/c/valheim/p/shudnal/TradersExtended/)<br/>
UI direction and structure based on the HaldorOverhaul project

[![GitHub](https://img.shields.io/badge/GitHub-Issues-181717?style=for-the-badge&logo=github)](https://github.com/JoeCorrell/HildirOverhaul/issues)

</div>
