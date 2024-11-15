using CommonHelper;

namespace AlgorithmHelper.Test
{
    public class CANCrcHelperTests
    {
        [Fact]
        public void CalculateCrc_ValidInput_ReturnsExpectedCrc()
        {
            // Arrange
            byte[] inputData = new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x0B, 0x00 };
            byte[] dataId = new byte[] { 237, 214, 150, 99, 165, 18, 213, 154, 30, 13, 36, 205, 140, 166, 47, 65 };
            byte expectedCrc = 0x6E; // 预期的CRC值

            // Act
            byte[] result = CANCrcHelper.CalculateCrc(inputData, dataId);

            // Assert
            Assert.Equal(expectedCrc, result[0]);
        }

        [Fact]
        public void CalculateCrc_InputDataLengthLessThanDataIdLength_ReturnsExpectedCrc()
        {
            // Arrange
            byte[] inputData = new byte[] { 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x0B, 0x00 };
            byte[] dataId = new byte[] { 237, 214, 150, 99, 165, 18, 213, 154, 30, 13, 36, 205, 140, 166, 47, 65 };
            byte expectedCrc = 0x24; // 预期的CRC值

            // Act
            byte[] result = CANCrcHelper.CalculateCrc(inputData, dataId);

            // Assert
            Assert.Equal(expectedCrc, result[0]);
        }

        [Fact]
        public void CalculateCrc_EmptyInputData_ReturnsInitialCrc()
        {
            // Arrange
            byte[] inputData = new byte[] { 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x0B, 0x00 };
            byte[] dataId = new byte[] { 237, 214, 150, 99, 165, 18, 213, 154, 30, 13, 36, 205, 140, 166, 47, 65 };
            byte expectedCrc = 0x0B; // 预期的CRC值

            // Act
            byte[] result = CANCrcHelper.CalculateCrc(inputData, dataId);

            // Assert
            Assert.Equal(expectedCrc, result[0]);
        }
    }
}