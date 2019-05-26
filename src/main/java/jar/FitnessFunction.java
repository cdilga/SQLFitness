package jar;

import io.jenetics.prog.ProgramGene;
import io.jenetics.prog.op.Op;
import org.apache.commons.math3.stat.descriptive.DescriptiveStatistics;
import org.bytedeco.javacpp.presets.opencv_core;
import org.deeplearning4j.models.embeddings.loader.WordVectorSerializer;
import org.deeplearning4j.models.word2vec.Word2Vec;

import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.function.Function;

import static jar.DBAccess.getConnection;

public class FitnessFunction {
    private Word2Vec vec;
    private ArrayList<String> questionWords;
    public FitnessFunction(ArrayList<String> keywords) {
        questionWords = keywords;
        vec = WordVectorSerializer.readWord2VecModel(Util.Word2VecPath);
    }

    private ArrayList<String> getCols(ProgramGene<String> program) {
        ArrayList<String> terms = new ArrayList<>();
        for (Op terminal: program.getTerminals()) {
            terms.add(terminal.toString().split("[<>]|(!=)|(==)|(<>)")[0]);
        }
        return terms;
    }

    public double getRowsRelevance(ResultSet result) throws SQLException {
        DescriptiveStatistics rowRelevance = new DescriptiveStatistics();
        if (result == null) {
            System.out.println("Result set is null.");
        }
        while (result.next()) {
            for (int i = 0; i < questionWords.size(); i++) {

                String word = result.getString(i + 1);
                Double value = vec.similarity(word.toLowerCase(), questionWords.get(i));
                rowRelevance.addValue(value);
            }
        }
        return rowRelevance.getMean();
    }

    public Function<ProgramGene<String>, Double> fitness = stringProgramGene -> {
        //Evaluate sql statement against DB

        try {
            //Get db connection
            //Get db statement

            //Generate full SQL from the program
            //Get resultset
            //Look at DBMiner code do some stuff
            Statement st = DBAccess.getConnection().createStatement();

            ArrayList<String> initialColumns = getCols(stringProgramGene);
            String sql = "SELECT " + String.join("," , initialColumns) + " FROM " + Util.tablename + " WHERE " + stringProgramGene.eval() + ";";
            ResultSet rs = st.executeQuery(sql);

            ResultSetMetaData rsmd = rs.getMetaData();
            int cols = rsmd.getColumnCount();
            String[] columns = new String[cols];

            //Initialise these to fixed len arrays which we will know getColumnnCount()
            //Loop through the columns from the metadata

            DescriptiveStatistics colRelevance = new DescriptiveStatistics();
            for (int i = 0; i < cols; i++) {
                columns[i] = rsmd.getColumnName(i+1).toLowerCase();
                for (int j =0; j< questionWords.size(); j++) {
                    colRelevance.addValue(vec.similarity(columns[i], questionWords.get(j)));
                }
            }

            //Implement row relevance
            return colRelevance.getMean() * getRowsRelevance(rs);
        } catch (SQLException e) {
            e.printStackTrace();
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        }

        return 0.0;
    };

}
