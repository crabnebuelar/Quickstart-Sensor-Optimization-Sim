Quantum Sensor Optimization Simulator
============================================================

Overview
----------------
This project is a Unity-based simulation that demonstrates how
quantum optimization algorithms can be applied to a sensor
placement and coverage problem.

The simulator allows users to place sensors in an environment,
define detection coverage, and run an optimization routine that
selects an optimal subset of sensors. The optimization is solved
using a Quadratic Unconstrained Binary Optimization (QUBO)
formulation and the Quantum Approximate Optimization Algorithm
(QAOA) via Qiskit.

While the Unity application handles visualization and interaction,
the optimization itself is performed by an external Python script.

Demo
-----------------------------
[Youtube](https://www.youtube.com/watch?v=0_gvtY9iuIQ)

Problem
-----------------------------
The central problem addressed is optimal sensor placement in facilities where incomplete coverage can lead to safety, security, or operational risks, while excessive sensor deployment increases cost, power consumption, and system complexity. This tradeoff results in a combinatorial optimization problem whose solution space grows exponentially with the number of candidate sensor locations. Classical optimization methods can struggle to efficiently explore this space as problem size increases. Quantum optimization algorithms, and hybrid approaches that combine quantum circuits with classical parameter optimization, offer a promising alternative by exploring the solution landscape in fundamentally different ways. This project uses QUBO formulation as a bridge between classical and quantum solution methods, enabling direct comparison and experimentation within a single framework.

What this simulator includes
-----------------------------
- **Digital Sensors and Detectables** (adjustable sensor range, wall obstruction)
- **Interactive UI with Drag-and-Drop Functionality** (user freedom of sensor/detectable placement)
- **Simulated Quantum Optimization** (QUBO-based optimization on qiskit's quantum simulator)
- **User-Selected Optimization Parameters** (provides lambda weights for QUBO terms)
- **Sample Facility Scenarios** (selection of three rooms with different layouts for simulation)

Quantum Optimization Approach
-----------------------------
The sensor selection problem is formulated as a QUBO:

- Binary variables represent whether each sensor is selected
- Linear terms penalize sensor cost
- Quadratic terms enforce coverage and overlap constraints
- Constant offsets normalize the objective

This QUBO is constructed using Qiskit's QuadraticProgram API and
solved using QAOA with a classical optimizer (COBYLA) and a
statevector simulator backend.

The output of the optimization is a list of selected sensors,
which is returned to Unity as a JSON file.


Python Dependencies
-------------------
This project requires Python 3.9+ and the following packages:

- qiskit
- qiskit-aer
- qiskit-algorithms
- qiskit-optimization
- numpy

Recommended installation method:

1. Install Miniconda or Anaconda
2. Create a virtual environment
3. Install dependencies using pip

Example commands:

    conda create -n quantum-opt python=3.10
    conda activate quantum-opt
    pip install qiskit qiskit-aer qiskit-algorithms qiskit-optimization

Quick Start (Unity)
--------------------------------------
To ensure the project runs on your computer:
 - In the editor, go to the Main Scene, and click on the SensorManager object. 
 - On the bottom script, in the Python Executable Path field, put the path for your python executable which has the qiskit dependencies installed.



How Unity Calls the Python Optimizer
-----------------------------------
Unity launches the Python optimizer as an external process and
passes arguments via the command line:

1. Coverage matrix (serialized as a string)
2. Output JSON file path
3. Lambda parameters for the QUBO objective

The Python script processes these arguments, runs the optimizer,
and writes the result to a JSON file, which Unity then reads.


Limitations
-----------
- Uses a simulated quantum backend (statevector)
- Performance scales poorly for large sensor counts
- Requires external Python installation
- QAOA parameters are fixed for simplicity


Project Intent
--------------
This project is intended as an educational and research-oriented
simulation demonstrating the integration of quantum optimization
techniques into an interactive Unity environment.

It is not intended for real-time or production-scale optimization.

Developed as part of a quantum optimization simulation project
integrating Unity and Qiskit.

============================================================
