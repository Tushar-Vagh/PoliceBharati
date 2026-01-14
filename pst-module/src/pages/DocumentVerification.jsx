import { useState } from "react";
import {
  Box,
  Button,
  Container,
  Grid,
  MenuItem,
  Paper,
  TextField,
  Typography,
} from "@mui/material";
import FactCheckIcon from "@mui/icons-material/FactCheck";

export default function DocumentVerification() {
  const [applicationNo, setApplicationNo] = useState("");
  const [documentType, setDocumentType] = useState("");
  const [status, setStatus] = useState("");
  const [result, setResult] = useState(null);
  const [error, setError] = useState("");

  const handleVerify = () => {
    setError("");
    setResult(null);

    if (!applicationNo || !documentType || !status) {
      setError("All fields are mandatory");
      return;
    }

    const verificationResult = {
      applicationNo,
      documentType,
      status,
      officerId: "OFFICER-1023",
      timestamp: new Date().toLocaleString(),
    };

    setResult(verificationResult);
  };

  return (
    <Container maxWidth={false} sx={{ height: 'calc(100vh - 100px)', overflow: 'hidden' }}>
      <Paper elevation={3} sx={{ p: 3, mt: 1, height: '100%', display: 'flex', flexDirection: 'column' }}>
        <Box sx={{ borderBottom: 1, borderColor: 'divider', pb: 1, mb: 2 }}>
          <Typography variant="h5" color="primary" sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
            <FactCheckIcon fontSize="large" /> Document Verification Module
          </Typography>
          <Typography variant="subtitle1" color="text.secondary" sx={{ mt: 1 }}>
            Verify candidate documents as per Government Resolution (GR).
          </Typography>
        </Box>

        <Grid container spacing={4}>
          <Grid item xs={12} lg={6}>
            <Paper variant="outlined" sx={{ p: 3, bgcolor: '#f8f9fa' }}>
              <Typography variant="h6" gutterBottom color="primary.dark">Verification Input</Typography>
              <Grid container spacing={3}>
                <Grid item xs={12}>
                  <TextField
                    fullWidth
                    label="Application Number"
                    placeholder="PB2026XXXX"
                    value={applicationNo}
                    onChange={(e) => setApplicationNo(e.target.value.toUpperCase())}
                  />
                </Grid>
                <Grid item xs={12} sm={6}>
                  <TextField
                    select
                    fullWidth
                    label="Document Type"
                    value={documentType}
                    onChange={(e) => setDocumentType(e.target.value)}
                  >
                    <MenuItem value="Caste Certificate">Caste Certificate</MenuItem>
                    <MenuItem value="Domicile Certificate">Domicile Certificate</MenuItem>
                    <MenuItem value="Non-Creamy Layer">Non-Creamy Layer</MenuItem>
                    <MenuItem value="Educational Certificate">Educational Certificate</MenuItem>
                  </TextField>
                </Grid>
                <Grid item xs={12} sm={6}>
                  <TextField
                    select
                    fullWidth
                    label="Verification Status"
                    value={status}
                    onChange={(e) => setStatus(e.target.value)}
                  >
                    <MenuItem value="PASS">Pass</MenuItem>
                    <MenuItem value="FAIL">Fail</MenuItem>
                  </TextField>
                </Grid>
                <Grid item xs={12}>
                  {error && (
                    <Typography color="error" variant="body2" gutterBottom>
                      {error}
                    </Typography>
                  )}
                  <Button variant="contained" size="large" onClick={handleVerify} fullWidth>
                    Submit Verification
                  </Button>
                </Grid>
              </Grid>
            </Paper>
          </Grid>

          {/* Result Section */}
          <Grid item xs={12} lg={6}>
            {result ? (
              <Paper
                elevation={4}
                sx={{
                  p: 4,
                  height: '100%',
                  bgcolor: result.status === "PASS" ? "#f1f8e9" : "#ffebee",
                  border: `2px solid ${result.status === "PASS" ? "#4caf50" : "#ef5350"}`,
                  position: 'relative',
                  overflow: 'hidden'
                }}
              >
                <Box sx={{ position: 'absolute', top: -10, right: -10, opacity: 0.1 }}>
                  <FactCheckIcon sx={{ fontSize: 200 }} />
                </Box>

                <Typography variant="h5" gutterBottom sx={{ borderBottom: '2px dashed grey', pb: 1 }}>
                  Official Verification Receipt
                </Typography>

                <Grid container spacing={3} sx={{ mt: 2 }}>
                  <Grid item xs={6}>
                    <Typography variant="caption" color="text.secondary">Application No</Typography>
                    <Typography variant="h6">{result.applicationNo}</Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="caption" color="text.secondary">Document</Typography>
                    <Typography variant="h6">{result.documentType}</Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="caption" color="text.secondary">Status</Typography>
                    <Typography variant="h4" color={result.status === "PASS" ? "success.main" : "error.main"} fontWeight="bold">
                      {result.status}
                    </Typography>
                  </Grid>
                  <Grid item xs={6}>
                    <Typography variant="caption" color="text.secondary">Officer ID</Typography>
                    <Typography variant="body1">{result.officerId}</Typography>
                  </Grid>
                  <Grid item xs={12}>
                    <Typography variant="caption" color="text.secondary">Timestamp</Typography>
                    <Typography variant="body2">{result.timestamp}</Typography>
                  </Grid>
                </Grid>
                <Box sx={{ mt: 4 }}>
                  {result.status === "PASS" ? (
                    <Typography color="success.main" variant="h6">
                      ✔ Document verified successfully.
                    </Typography>
                  ) : (
                    <Typography color="error" variant="h6">
                      ✖ Document verification failed.
                    </Typography>
                  )}
                </Box>
              </Paper>
            ) : (
              <Box sx={{
                height: '100%',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                bgcolor: 'background.default',
                borderRadius: 2,
                border: '2px dashed #e0e0e0',
                color: 'text.secondary'
              }}>
                <Typography>Verification Receipt will appear here</Typography>
              </Box>
            )}
          </Grid>
        </Grid>
      </Paper>
    </Container>
  );
}
