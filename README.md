# ChicksGold-BE-Challenge
API that can compute the steps required to measure exactly Z gallons using two jugs of capacities X and Y gallons

# solucion
    - pense en resolver el problema usando backtracking como mencione en la entrevista, pero realmente luego de leer bien el enunciado me he dado cuenta que no seria lo mas eficiente.
    - se tomara en cuenta que la app solo aceptara valores positivos, y se rechazara valores negativos o nulos
        -esto podria mejorarse multiplicando por -1 todo valor negativo, aplicando valor absoluto

    -propongo que el API devuelva todas las posibles soluciones, además de identificar y retornar explícitamente la mejor y la peor solución y no solo como esta en sample response.json
        {
            "solution": [
                {"step": 1, "bucketX": 2, "bucketY": 0, "action": "Fill bucket X"},
                {"step": 2, "bucketX": 0, "bucketY": 2, "action": "Transfer from bucket X to Y"},
                {"step": 3, "bucketX": 2, "bucketY": 2, "action": "Fill bucket X"},
                {"step": 4, "bucketX": 0, "bucketY": 4, "action": "Transfer from bucket X to Y", "status": "Solved"}
            ]
        }
    - Buscando ofrecer una respuesta más completa y flexible, se incluye en el response.
      - Un arreglo con todas las soluciones posibles (cada una como una lista de pasos).
      - Un campo para la mejor solución (la de menos pasos).
      - Un campo para la peor solución (la de más pasos).