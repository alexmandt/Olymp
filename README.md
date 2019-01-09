# 🏔️ Olymp

## A distributed compute engine that allows you to focus on *your* work

### Get started

#### Run master node

```bash
$ olymp --master --name master.local --webui
```

#### Run child node

```bash
$ olymp --child --name child1.local 
```

#### Connect to master node CLI configuration

```bash
$ olymp --configure localhost:17929
master.local>
```

#### Upload DLL to master node

We will upload a simple calculator program that we wrote.

```bash
master.local> put worker program "/home/u1/calc.dll" as "calculator"
```

#### Distribute DLL to child nodes

```bash
master.local> distribute "calculator" to "child1.local"
master.local> distribute "calculator" to "child2.local"
```

#### Write a simple pipeline

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

#### Deploy the pipeline

```bash
master.local> put master pipeline "/home/u1/add.js"
```

### Default ports

* Configuration: 17929