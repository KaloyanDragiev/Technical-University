namespace Calculator
{
    public class Matrix
    {
        private int n; 
        private int k; 
        private double[,] matrix;
        public Matrix(int rows, int columns)
        {
            this.n = rows;
            this.k = columns;
            this.matrix = new double[this.k, this.n];
        }

        public Matrix(double[,] newMatrix)
        {
            this.n = newMatrix.GetLength(1);
            this.k = newMatrix.GetLength(0);
            this.setMatrix(newMatrix);
        }
        public bool setMatrix(double[,] newMatrix)
        {
            if (newMatrix.GetLength(0) == this.k && newMatrix.GetLength(1) == this.n)
            {
                this.matrix = newMatrix;
                return true;
            }
            return false;
        }

        private bool isSquare()
        {
            return this.n == this.k;
        }
        private double[] getColumn(int index)
        {
            double[] c = new double[this.k];
            for (int i = 0; i < this.n; i++)
            {
                for (int j = 0; j < this.k; j++)
                {
                    if (j == index)
                    {
                        c[i] = this.matrix[i, index];
                    }
                }
            }
            return c;
        }

        private void addToMatrixRow(int rowID, int adderID, double multiplier)
        {
            for (int m = 0; m < this.k; m++)
            {
                this.matrix[rowID, m] += multiplier * this.matrix[adderID, m];
            }
        }
        public double[] coeffOfPolynomial(double[] results)
        {
            double[] ret = new double[results.Length];
            for (int i = 0; i < results.Length; i++)
            {
                ret[i] = results[i];
            }
            double[,] mat = this.matrix;
            if (this.isSquare() && ret.Length == this.n)
            {
                for (int i = 0; i < this.n; i++)
                {
                    for (int j = 0; j < this.k; j++)
                    {
                        if (i == j)
                        {
                            if (this.matrix[i, j] == 0)
                            {
                                int nextRow = (i == this.n - 1 ? 0 : i + 1);
                                this.addToMatrixRow(i, nextRow, 1);
                                ret[i] += ret[nextRow];
                            }
                            double firstNumOfRow = this.matrix[i, j];//
                            for (int m = 0; m < this.n; m++)
                            {
                                if (m != i)
                                {
                                    double firstNumOfNextRow = this.matrix[m, j];
                                    double mult = (-1) * (firstNumOfNextRow / firstNumOfRow);
                                    this.addToMatrixRow(m, i, mult);
                                    ret[m] += mult * ret[i];
                                }
                            }
                        }
                    }
                }
                for (int i = 0; i < this.n; i++)
                {
                    double mult = 1 / this.matrix[i, i];
                    ret[i] = mult * ret[i];
                }

                this.matrix = mat;
                return ret;
            }
            return null;
        }

        static Matrix unitMatrixWithSize(int size)
        {
            double[,] m = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (i == j) { m[i, j] = 1; }
                    else { m[i, j] = 0; }
                }
            }
            return new Matrix(m);
        }
    }
}
