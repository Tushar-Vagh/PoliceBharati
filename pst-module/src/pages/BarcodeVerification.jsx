import { useState } from "react";
import {
  Box,
  Button,
  Container,
  Grid,
  Paper,
  TextField,
  Typography,
} from "@mui/material";
import QrCodeScannerIcon from "@mui/icons-material/QrCodeScanner";

export default function BarcodeVerification() {
  const [applicationNo, setApplicationNo] = useState("");
  const [candidate, setCandidate] = useState(null);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const isValidApplicationNo = (value) => {
    const pattern = /^PB2026\d{4}$/;
    return pattern.test(value);
  };

  const handleScan = () => {
    setError("");
    setCandidate(null);

    const trimmedAppNo = applicationNo.trim();

    if (!trimmedAppNo) {
      setError("Application number is required");
      return;
    }

    if (!isValidApplicationNo(trimmedAppNo)) {
      setError("Invalid Application Number format (Expected: PB2026XXXX)");
      return;
    }

    setLoading(true);

    const mockDatabase = {
      PB20260001: {
        name: "Rahul Patil",
        gender: "Male",
        category: "Open",
        dob: "1999-04-12",
        eventStatus: "Physical Measurement Test",
      },
      PB20260002: {
        name: "Sneha Deshmukh",
        gender: "Female",
        category: "OBC",
        dob: "2000-08-25",
        eventStatus: "Written Examination",
      },
    };

    setTimeout(() => {
      if (mockDatabase[trimmedAppNo]) {
        setCandidate(mockDatabase[trimmedAppNo]);
      } else {
        setError("Application number not found in system");
      }
      setLoading(false);
    }, 700);
  };

  return (
    <Container maxWidth={false}>
      <Paper sx={{ p: 4, mt: 2, minHeight: '80vh' }}>
        <Box sx={{ borderBottom: 1, borderColor: 'divider', pb: 2, mb: 4 }}>
          <Typography variant="h4" color="primary" sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
            <QrCodeScannerIcon fontSize="large" /> Barcode / QR Identity Check
          </Typography>
        </Box>

        <Grid container spacing={4} justifyContent="center" alignItems="flex-start">
          <Grid item xs={12} lg={5}>
            <Paper variant="outlined" sx={{ p: 4, bgcolor: '#f5f5f5', textAlign: 'center' }}>
              <Typography variant="h6" gutterBottom>Scan Station</Typography>
              <Box sx={{ my: 4, display: 'flex', justifyContent: 'center' }}>
                <QrCodeScannerIcon sx={{ fontSize: 120, color: 'text.disabled' }} />
              </Box>
              <TextField
                fullWidth
                label="Scan Barcode / QR Code"
                placeholder="PB2026XXXX"
                value={applicationNo}
                onChange={(e) => {
                  setApplicationNo(e.target.value.toUpperCase());
                  setError("");
                }}
                error={!!error}
                helperText={error}
                sx={{ mb: 2 }}
              />
              <Button
                fullWidth
                variant="contained"
                size="large"
                onClick={handleScan}
                disabled={loading}
                sx={{ height: 55 }}
              >
                {loading ? "Verifying..." : "Verify Candidate"}
              </Button>
            </Paper>
          </Grid>

          <Grid item xs={12} lg={7}>
            {candidate ? (
              <Paper
                elevation={3}
                sx={{
                  p: 4,
                  height: '100%',
                  bgcolor: "white",
                  borderLeft: "8px solid #1a237e",
                }}
              >
                <Typography variant="overline" color="text.secondary">Identity Record Found</Typography>
                <Typography variant="h4" gutterBottom color="primary.dark" sx={{ mb: 4 }}>
                  {candidate.name}
                </Typography>

                <Grid container spacing={4}>
                  <Grid item xs={12} sm={4}>
                    <Typography variant="caption" display="block" color="text.secondary">Gender</Typography>
                    <Typography variant="h6">{candidate.gender}</Typography>
                  </Grid>
                  <Grid item xs={12} sm={4}>
                    <Typography variant="caption" display="block" color="text.secondary">Category</Typography>
                    <Typography variant="h6">{candidate.category}</Typography>
                  </Grid>
                  <Grid item xs={12} sm={4}>
                    <Typography variant="caption" display="block" color="text.secondary">Date of Birth</Typography>
                    <Typography variant="h6">{candidate.dob}</Typography>
                  </Grid>
                  <Grid item xs={12}>
                    <Divider sx={{ my: 2 }} />
                    <Typography variant="caption" display="block" color="text.secondary">Current Event Status</Typography>
                    <Typography variant="h5" color="secondary.main" fontWeight="bold">
                      {candidate.eventStatus}
                    </Typography>
                  </Grid>
                </Grid>

                <Box sx={{ mt: 4, p: 2, bgcolor: '#e8f5e9', borderRadius: 1, color: 'success.dark', display: 'flex', alignItems: 'center', gap: 1 }}>
                  <Typography fontWeight="bold">✔ VERIFIED</Typography>
                  <Typography variant="body2">Identity confirmed against central database.</Typography>
                </Box>
              </Paper>
            ) : (
              <Box sx={{
                height: '100%',
                minHeight: 400,
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                border: '2px dashed #e0e0e0',
                borderRadius: 2
              }}>
                <Typography color="text.secondary">Candidate details will appear here after scanning</Typography>
              </Box>
            )}
          </Grid>
        </Grid>
      </Paper>
    </Container>
  );
}
