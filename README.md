![](/assets/logo-wide.png)

<!-- Badges here -->
[![Azure Build Status](https://dev.azure.com/arielsimulevski0813/arielsimulevski/_apis/build/status/Azer0s.Olymp?branchName=dev)](https://dev.azure.com/arielsimulevski0813/arielsimulevski/_build/latest?definitionId=1&branchName=dev)  [![Travis Build Status](https://travis-ci.org/Azer0s/Olymp.svg?branch=dev)](https://travis-ci.org/Azer0s/Olymp)  [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/Azer0s/Olymp/blob/master/README.md)

Olymp is an open source distributed compute engine which allows you to build and deploy distributed compute applications fast without writing any boilerplate communication code. Write an application, upload it to your private Olymp cloud and look at it go!

Olymp supports multiple languages & runtimes (like Node, C#, Java, Python or Ruby). This means that you can write certain parts of your application in the language which is best suitable for it. Stop limiting yourself to one language when you can use the right tool for the job.

Currently, we are in pre-alpha stage. To stay updated, just leave a star.

## Getting started

### Run master node

**Master machine (IP: 192.168.0.100)**

```bash
$ olymp --master --name master.local --webui
```

### Run child nodes

**Machine 1**

```bash
$ olymp --child 192.168.0.100:17930 --name child1.local --user admin --password admin
```

**Machine 2**

```bash
$ olymp --child 192.168.0.100:17930 --name child2.local --user admin --password admin
```

### Connect to master node CLI configuration

```bash
$ olymp --configure localhost:17929 --user admin --password admin
master.local>
```

### Upload DLL to master node

We will upload a simple calculator program that we wrote.

```bash
master.local> put program "/home/u1/calc.dll" as "calculator"
```

### Distribute DLL to child nodes

```bash
master.local> distribute "calculator" to "child1.local"
master.local> distribute "calculator" to "child2.local"
```

### Write a simple pipeline

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

### Deploy the pipeline

```bash
master.local> put pipeline "/home/u1/add.js" as "add"
master.local> distribute "add" to self
```

### Default ports (master)

* Configuration: 17929
* Child connections: 17930

## Contributing

[![GitHub issues by-label](https://img.shields.io/github/issues/Azer0s/Olymp/good%20first%20issue.svg)](https://img.shields.io/github/issues/Azer0s/Olymp/good%20first%20issue.svg)
