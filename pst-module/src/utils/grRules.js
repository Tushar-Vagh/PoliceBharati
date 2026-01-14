export const GR_STANDARDS = {
  MALE: {
    minHeight: 165.0,
    minChest: 79.0,
    minWeight: 50.0
  },
  FEMALE: {
    minHeight: 155.0,
    minChest: 0.0, // Usually not applicable
    minWeight: 45.0
  }
};

export const evaluatePST = (data) => {
  const h = parseFloat(data.height_cm);
  const c = parseFloat(data.chest_cm);
  const w = parseFloat(data.weight_kg);
  const gender = data.gender || 'Male'; // Default to Male if not specified

  const standards = gender === 'Female' ? GR_STANDARDS.FEMALE : GR_STANDARDS.MALE;

  const isHeightPass = h >= standards.minHeight;
  const isWeightPass = w >= standards.minWeight;
  // For females, chest might not be a criteria, or we can just skip it if minChest is 0
  const isChestPass = standards.minChest === 0 || c >= standards.minChest;

  const isPass = isHeightPass && isWeightPass && isChestPass;

  const reasons = [];
  if (!isHeightPass) reasons.push('height');
  if (!isChestPass) reasons.push('chest');
  // if (!isWeightPass) reasons.push('weight'); // Assuming weight isn't a rejection criteria in the slip, or add it if needed.

  return {
    status: isPass ? "Pass" : "Fail",
    reasons: reasons
  };
};