SELECT 
	Spouse.Names, Gender.GenderType, Spouse.EmailAddress, Spouse.Number
from	Spouse, Gender
where Spouse.GenderType = GenderId;