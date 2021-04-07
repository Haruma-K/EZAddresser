<h1 align="center">EZAddresser</h1>

EZAddresser is automatic addressing system for Unity Addressable Asset System.
You can load assets in very simple two steps:

1. Put the asset you want to load into the "Addressables" folder.
2. Call `Addressables.LoadAssetAsync("[asset name here]")`.

<p align="center">
  <img src="https://user-images.githubusercontent.com/47441314/105809345-de370380-5fec-11eb-84fb-e1511e653b0b.gif" alt="Sample">
</p>

## Requirement
Unity 2020.1 or higher (because of the generic type serialization).

## Install
1. Open the Package Manager from Window > Package Manager
2. "+" button > Add package from git URL
3. Enter the following
    * https://github.com/Haruma-K/EZAddresser.git?path=/Packages/com.harumak.ezaddresser

<p align="center">
  <img width=500 src="https://user-images.githubusercontent.com/47441314/113792801-c1077980-9781-11eb-8770-210999a25c9a.png" alt="Package Manager">
</p>


Or, open Packages/manifest.json and add the following to the dependencies block.

```json
{
    "dependencies": {
        "com.harumak.ezaddresser": "https://github.com/Haruma-K/EZAddresser.git?path=/Packages/com.harumak.ezaddresser"
    }
}
```

If you want to set the target version, specify it like follow.
* https://github.com/Haruma-K/EZAddresser.git?path=/Packages/com.harumak.ezaddresser#0.1.0

## Getting Started
You can load the assets with EZAddresser by following steps:

1. Create a folder named "Addressables".
2. Store the assets you want to load in the "Addressables" folder.
3. Call Addressables.LoadAssetAsync([file name]) to load them.

In this process, addresses and groups are created according to the following rules.

1. Addresses will be created to match the file name without extension. (This can be changed, see below.)
2. Multiple "Addressables" folders can be created and folders can be nested together.
3. the AssetBundle will be split per "Addressables" folders. (This can be changed, see below.)

## Global Settings
You can edit the global settings from Window > EZAddresser > Settings.

<p align="center">
  <img src="https://user-images.githubusercontent.com/47441314/105818382-345e7380-5ffa-11eb-94a3-e98657967a0f.png" alt="Global Settings">
</p>

The description of each setting item is as follows.

|Item Name|Description|
|-|-|
|Base Packing Mode|<b>Pack By Addressables Folder</b>: AssetBundle is split according to the "Addressables" folder.<br><b>Pack Together</b>: All assets are stored in the same AssetBundle.|
|Base Addressing Mode|<b>Asset Name</b>: File names with extensions are used for addresses.<br><b>Asset Name Without Extensions</b>: File names without extensions are used for addresses.<br><b>Addressable Path:</b> Relative paths from the "Addressables" folder with extensions are used for addresses.<br><b>Addressable Path Without Extensions:</b> Relative paths from the "Addressables" folder without extensions are used for addresses.|
|Group Template|Template used to create a group.<br>If not set, the first template set in AddressableAssetSettings will be used.|

When you change these setting items, the following dialog will appear and all addresses and groups will be updated.

<p align="center">
  <img src="https://user-images.githubusercontent.com/47441314/105822625-a4bbc380-5fff-11eb-80e6-929ed446d915.png" alt="Dialog">
</p>

Note that when you change the Group Template, all groups will be deleted once and the settings for each group will be reset.

## Per Asset Settings
You can edit the per asset settings from Window > EZAddresser > Entry Rule Editor.

<p align="center">
  <img src="https://user-images.githubusercontent.com/47441314/106751584-bab72d00-666c-11eb-9d26-55d85a66419a.png" alt="Per Asset Settings">
</p>

You can create a new rule from the Create button on the toolbar.  
The description of each setting item is as follows.

|Item Name|Description|
|-|-|
|Addressable Path Rule|Regular expression representing the relative path of the target asset from the Addressables folder.|
|Addressing Mode|Addressing Mode to be applied to the target asset.|
|Group Name Rule|Group name to which the target asset belongs.<br>The actual group name is retrieved with `Regex.Replace([Addressable Path], [Addressable Path Rule], [Group Name Rule])`.<br>However, '/' is replaced by '-' because it is not an appropriate group name.|
|Label Rule(s)|The name of the label to attach to the target asset.<br>The actual label is retrieved with `Regex.Replace([Addressable Path], [Addressable Path Rule], [Label Rule])`.<br>Multiple labels can be defined by separating them with commas.<br>Spaces are not allowed in label names.|

For example, if you set the Addressable Path Rule as prepub_sample_(?<prefab_id>[0-9]{3})\.prefab, then
Both prefab_sample_001.prefab and prefab_sample_002.prefab will be subject to this rule.
And if you set the Group Name Rule of this rule as Prefab${prefab_id}, the above Prefabs will be stored in the groups Prefab001 and Prefab002 respectively.

If you change any of these settings, the following dialog will appear and all addresses and groups will be updated.

<p align="center">
  <img src="https://user-images.githubusercontent.com/47441314/105823884-11838d80-6001-11eb-996b-d62a962082f2.png" alt="Dialog">
</p>

## Assumed Workflow
EZAddresser is designed assuming the following workflow.

The first thing to think about is the prototype or early stage of the project.
At this time, it's enough to have all the resources built into the app, and you don't need to think about how to divide the AssetBundle.
Therefore, you can just throw the resources you want to load into the Addressables folder without thinking about the details.
But you may want to decide only the Base Addressing Mode in Settings.

As the project progresses, a resource delivery server will be prepared to download resources.
At this point, you will set up Content Packing & Loading for each group using the Addressalbe asset system features.
Also, if you want to change the template for a group, it is recommended to do so at this point.

And as the project gets closer to completion, one of the issues that often comes up is the division unit of the AssetBundle.  
Most of the time, you will need to split an asset bundle that is too large into appropriate granularity.  
At this point, you can use the Entry Rule Editor to store the assets into the appropriate AssetBundle.

## Operate programmatically
Global settings and per-asset settings can be operated programmatically.

The global settings can be operated as follows.

```cs
var settingsService = new SettingsService(new SettingsRepository());
settingsService.UpdateSetting(packingMode, addressingMode, groupTemplateGuid);
settingsService.Save();
```

Per asset settings can be operated as follows.

```cs
var entryRulesService = new EntryRulesService(new EntryRulesRepository());

// Get all rules.
var allRules = entryRulesService.GetState();
// Get the first rule.
var firstRule = allRules.First();
// Update the first rule.
entryRulesService.UpdateRule(firstRule.Id, new EntryRuleUpdateCommand(addressablePathRule, addressingMode, groupNameRule));
// Add rule.
entryRulesService.AddRule(new EntryRuleUpdateCommand(addressablePathRule, addressingMode, groupNameRule));
// Remove Rule.
entryRulesService.RemoveRule(firstRule.Id);
// Save changes.
entryRulesService.Save();
```


## Lisence
This software is released under the MIT License, see LICENSE.md.
