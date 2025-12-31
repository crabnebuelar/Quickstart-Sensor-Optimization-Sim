import json
import sys
import os
from qiskit_aer import Aer
from qiskit_ibm_runtime import QiskitRuntimeService, SamplerV2
from qiskit.primitives import StatevectorSampler
from qiskit_algorithms import QAOA
from qiskit_optimization import QuadraticProgram
from qiskit_optimization.algorithms import MinimumEigenOptimizer
from qiskit_algorithms.optimizers import COBYLA


'''try:
    print("PYTHON STARTED")
    sys.stdout.flush()

    print("CWD:", os.getcwd())
    sys.stdout.flush()

    print("ARGV:", sys.argv)
    sys.stdout.flush()

except Exception as e:
    with open("python_boot_error.txt", "w") as f:
        f.write(str(e))
    raise'''

test_path = sys.argv[2] if len(sys.argv) > 2 else "test_output.txt"

with open(test_path, "w") as f:
    f.write("Python reached file write stage\n")

print("Wrote test file to:", test_path)
sys.stdout.flush()



C = [[int(x) for x in row.split(",")]
    for row in sys.argv[1].split(";")]

output_path = sys.argv[2]



'''C = [
    [1,1,0,0,0,0],  # S1
    [0,1,1,0,0,0],  # S2
    [0,0,1,1,1,0],  # S3
    [0,0,0,0,1,1],  # S4
]'''

lambda1 = 5
lambda2 = 5
lambda3 = 10

num_sensors = len(C)
num_positions = len(C[0])

# Define small QUBO
qp = QuadraticProgram()
for i in range(num_sensors):
    qp.binary_var(f"x{i+1}")

# qp.minimize(linear={f"x{i+1}": lambda1 for i in range(4)})

linear_obj = {f"x{i+1}": lambda1 for i in range(num_sensors)}  # sensor cost
quadratic_obj = {}
constant_obj = 0

# Add coverage penalties
for j in range(num_positions):
    sensors_covering_j = [i for i in range(num_sensors) if C[i][j]==1]
    # Linear
    for i in sensors_covering_j:
        linear_obj[f"x{i+1}"] = linear_obj.get(f"x{i+1}",0) - 2*lambda2
    # Quadratic
    for i in range(len(sensors_covering_j)):
        for k in range(i+1, len(sensors_covering_j)):
            a = sensors_covering_j[i]
            b = sensors_covering_j[k]
            quadratic_obj[(f"x{a+1}", f"x{b+1}")] = 2*lambda3
    # Constant
    constant_obj += lambda2

# Call minimize once
qp.minimize(linear=linear_obj, quadratic=quadratic_obj, constant=constant_obj)


# 2. Choose QAOA optimizer
# service = QiskitRuntimeService(channel="ibm_cloud")

# 2. Initialize sampler (no backend argument)
sampler = StatevectorSampler()
qaoa = QAOA(sampler=sampler, optimizer=COBYLA(maxiter=50), reps=1)
meo = MinimumEigenOptimizer(qaoa)

# 3. Solve
result = meo.solve(qp)

solution = {
    "selected_sensors": [
        {"key": f"S{i}", "value": float(result[f"x{i+1}"])}
        for i in range(num_sensors)
    ]
}

with open(output_path, "w") as f:
    json.dump(solution, f, indent=2)