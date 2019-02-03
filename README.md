![](/assets/logo-wide.png)

<!-- Badges here -->
[![Azure Build Status](https://dev.azure.com/arielsimulevski0813/arielsimulevski/_apis/build/status/Azer0s.Olymp?branchName=dev)](https://dev.azure.com/arielsimulevski0813/arielsimulevski/_build/latest?definitionId=1&branchName=dev)
[![Travis Build Status](https://travis-ci.org/Azer0s/Olymp.svg?branch=dev)](https://travis-ci.org/Azer0s/Olymp)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/Azer0s/Olymp/blob/master/README.md)
[![made-with-csharp](https://img.shields.io/badge/Made%20with-C%23-blue.svg)](https://de.wikipedia.org/wiki/C-Sharp)
[![Built with](https://img.shields.io/badge/Built%20with-%20%F0%9F%96%A4%20-red.svg)](https://img.shields.io/badge/Built%20with-%20%F0%9F%96%A4%20-red.svg)

Olymp is an open source distributed compute engine which allows you to build and deploy distributed compute applications fast without writing any boilerplate communication code. Write an application, upload it to your private Olymp cloud and look at it go!

Olymp supports multiple languages & runtimes (like Node, C#, Java, Python or Ruby). This means that you can write certain parts of your application in the language which is best suitable for it. Stop limiting yourself to one language when you can use the right tool for the job.

Currently, we are in pre-alpha stage. To stay updated, just leave a star.

## Getting started :chart_with_upwards_trend:

### Run master node :runner: :older_man:

**Master machine (IP: 192.168.0.100)**

```bash
$ olymp --master --name master.local --webui
```

### Run child nodes :runner: :baby: :baby:

**Machine 1**

```bash
$ olymp --child olymp://192.168.0.100/connect --name child1.local --user admin --password admin
```

or with a simple connection string:

```bash
$ olymp --child olymp://192.168.0.100/connect?user=admin&password=admin --name child1.local
```

**Machine 2**

```bash
$ olymp --child olymp://192.168.0.100:17930/connect --name child2.local --user admin --password admin
```

### Connect to master node CLI configuration :electric_plug: :older_man:

```bash
$ olymp --configure olymp://localhost/configure --user admin --password admin
master.local>
```

### Upload  worker program to master node :arrow_up: :construction_worker:

We will upload a simple calculator program that we wrote.

```bash
master.local> put program "/home/u1/calc.dll" as "calculator"
```

### Distribute worker program to child nodes :ship: :construction_worker:

```bash
master.local> distribute "calculator" to "child1.local"
master.local> distribute "calculator" to "child2.local"
```

### Write a simple pipeline ✍️ →:envelope:→

This pipeline can access our calculator program and use it from our master node.

```js
function add(w,x,y,z){
    var node1 = getNode("child1.local");
    var node2 = getNode("child2.local");
    var c1 = node1.getWorker("calculator");
    var c2 = node2.getWorker("calculator");
    
    simultan([
        function(){r1 = c1.add(w,x);},
        function(){r2 = c2.add(y,z);}
    ]);
    
    return r1 + r2;
}
```

### Deploy the pipeline :arrow_up: →:envelope:→
```bash
master.local> put pipeline "/home/u1/add.js" as "add"
master.local> distribute "add" to self
```

<!--### Default ports (master)

* Configuration: 17929
* Child connections: 17930
TODO: Move to wiki -->

## Contributors ✨

<!-- prettier-ignore -->
| [<img src="https://avatars1.githubusercontent.com/u/16290284?s=460&v=4" width="100px;" alt="Ariel Simulevski"/><br /><sub><b>Ariel Simulevski</b></sub>](https://github.com/Azer0s)<br />[💻](https://github.com/Azer0s/Olymp/commits?author=Azer0s "Code")[🤔](https://github.com/Azer0s/Olymp/issues/created_by/Azer0s "Issues & Ideas")[🚧](#admin "Owner")[💬](#qa-Azer0s "Q&A")[👀](https://github.com/Azer0s/Olymp/pulls?utf8=%E2%9C%93&q=is%3Apr+reviewed-by%3AAzer0s+ "Reviews") | [<img src="https://avatars2.githubusercontent.com/u/26521741?s=460&v=4" width="100px;" alt="Dimitar Rusev"/><br /><sub><b>Dimitar Rusev</b></sub>](https://github.com/Mitiko)<br />[💻](https://github.com/Azer0s/Olymp/commits?author=Mitiko "Code")[🤔](https://github.com/Azer0s/Olymp/issues/created_by/Mitiko "Issues & Ideas")[💬](#qa-Mitiko "Q&A")[🔧](https://github.com/Mitiko/OlympTools "Tools")| [<img src="https://avatars1.githubusercontent.com/u/16230651?s=460&v=4" width="100px;" alt="Ali Sheikh"/><br /><sub><b>Ali Sheikh</b></sub>](https://github.com/alaeschaik)<br />[💻](https://github.com/Azer0s/Olymp/commits?author=alaeschaik "Code")[🎨](#design-alaeschaik "Design")[💬](#qa-alaeschaik "Q&A")[⚠️](#tests-alaeschaik "Testing") |
| :---: | :---: | :---: |

## Contributing

[![GitHub issues by-label](https://img.shields.io/github/issues/Azer0s/Olymp/good%20first%20issue.svg)](https://img.shields.io/github/issues/Azer0s/Olymp/good%20first%20issue.svg)

<!--🔜🔛🔝-->
