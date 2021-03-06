SELECT   PrescMedicationId As Identifier,
		 NameOrDesc AS NameOrDescription,
         Posology,
         Quantity,
         Duration,
         Code
FROM     PrescMedication
WHERE    PrescriptionId = @PrescriptionId
ORDER BY NameOrDesc