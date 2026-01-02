============================================================
QUANTUM SENSOR OPTIMIZATION SIMULATOR
README
============================================================

PROJECT OVERVIEW
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


QUANTUM OPTIMIZATION APPROACH
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


PYTHON DEPENDENCIES
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

MAKING THE PROJECT WORK ON ANY MACHINE
--------------------------------------
To ensure the project runs on your computer:
 - In the editor, go to the Main Scene, and click on the SensorManager object. 
 - On the bottom script, in the Python Executable Path field, put the path for your python executable which has the qiskit dependencies installed.



HOW UNITY CALLS THE PYTHON OPTIMIZER
-----------------------------------
Unity launches the Python optimizer as an external process and
passes arguments via the command line:

1. Coverage matrix (serialized as a string)
2. Output JSON file path
3. Lambda parameters for the QUBO objective

The Python script processes these arguments, runs the optimizer,
and writes the result to a JSON file, which Unity then reads.


LIMITATIONS
-----------
- Uses a simulated quantum backend (statevector)
- Performance scales poorly for large sensor counts
- Requires external Python installation
- QAOA parameters are fixed for simplicity


PROJECT INTENT
--------------
This project is intended as an educational and research-oriented
simulation demonstrating the integration of quantum optimization
techniques into an interactive Unity environment.

It is not intended for real-time or production-scale optimization.


AUTHOR
------
Developed as part of a quantum optimization simulation project
integrating Unity and Qiskit.

============================================================
