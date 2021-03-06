SELECT   PrescriptionId AS Identifier,
         CASE Status
			WHEN 'CRT' THEN 0
			WHEN 'INP' THEN 1
			WHEN 'DLV' THEN 2
			WHEN 'RVK' THEN 3
			WHEN 'EXP' THEN 4
			WHEN 'ARC' THEN 5
		 END AS Status,
         CreatedOn,
         DelivrableAt,
		 PrescriberDisplayName
FROM     Prescription
WHERE    PrescriptionType = 'PHARM'
AND      PatientId = @PatientId
ORDER BY CreatedOn, PrescriptionId